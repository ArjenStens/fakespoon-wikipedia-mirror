using System.Text.RegularExpressions;
using FakeSpoon.Lib.Cqe.Base;
using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Events.Tags;
using FakeSpoon.Lib.NostrClient.Events.Values;
using FakeSpoon.Wikipedia.Mirror.Domain.Nostr.Models.Tags;
using FakeSpoon.Wikipedia.Mirror.Domain.WikiFreedia.Utils;
using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;
using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Utils;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class CreateWikiFreediaNoteCommand : ICommand
{
    required public WikiPage WikiPage { get; set; }
}

public class CreateWikiFreediaNoteCommandHandler(
    ILogger<CreateWikiFreediaNoteCommandHandler> Logger,
    ICommandHandler<PubCommand> handler) : ICommandHandler<CreateWikiFreediaNoteCommand>
{
    public async Task Execute(CreateWikiFreediaNoteCommand cmd)
    {
        Logger.LogInformation("page");

        var categories = GetCategories(cmd.WikiPage.Revision.Text.Value);
        
        var markdownContent = await WikiConverter.ToWikiArticle(cmd.WikiPage.Revision.Text.Value);

        markdownContent += $"\n View this article on [legacy Wikipedia]({WikiPediaUtils.UrlFromTitle(cmd.WikiPage.Title)})";
        var note = new NostrEvent
        {
            Kind = Kind.WikiArticle,
            Tags = new INostrTag[]
            {
                new IdentifierTag(WikiFreediaUtils.AsTopicName(cmd.WikiPage.Title)),
                new TitleTag(cmd.WikiPage.Title),
                new CategoriesTag(categories),
                new ClientTag("FakeSpoon-WikiMirror", new PublicKeyValue("bla"), "fakespoon-wiki-mirror", null)
            },
            Content = new(markdownContent)
        };

        await handler.Execute(new PubCommand {Event = note});
        
    }

    public IEnumerable<string> GetCategories(string wikiText)
    {
        var categories = new List<string>();

        string? currentLine;
        var strReader = new StringReader(wikiText);
        while(true)
        {
            currentLine = strReader.ReadLine();

            if (currentLine is null)
            {
                break;
            }

            if (!currentLine.StartsWith("[[Category:")) continue;
            
            var categoryName = currentLine[11..(currentLine.Length - 2)]; // minus closing ]]
            categories.Add(categoryName);
        }
        
        return categories;
    }
    
    public string GetMarkdownContent(string wikiText)
    {
        var markdownLines = new List<string>();

        string? currentLine;
        var strReader = new StringReader(wikiText);
        while(true)
        {
            currentLine = strReader.ReadLine();

            if (currentLine is null || currentLine.StartsWith("==References=="))
            {
                break;
            }

            // header
            if (currentLine.StartsWith("==") && currentLine.EndsWith("=="))
            {
                markdownLines.Add($"## {currentLine[2..(currentLine.Length - 2)]}"); // minus closing ==);
                continue;
            }

            var lineContent = GetLineContent(currentLine);
            markdownLines.Add(lineContent);
        }
        
        return string.Join("\n",markdownLines);
    }

    public string GetLineContent(string wikiLine)
    {
        // References
        // Reverse loop to prevent indexes from moving during iteration!
        var referencePattern = "<ref[\\s\\S]*?<\\/ref>";
        foreach (Match match in Regex.Matches(wikiLine, referencePattern, RegexOptions.IgnoreCase).Reverse())
        {
            var fullMatch = match.Value;
            wikiLine = wikiLine.Replace(fullMatch, "[omitted_ref]"); // TODO: parse this
        }
        
        // Wikilinks
        var wikiLinkPattern = "\\[\\[[\\s\\S]*?\\]\\]";
        foreach (Match match in Regex.Matches(wikiLine, wikiLinkPattern, RegexOptions.IgnoreCase).Reverse())
        {
            var wikiLink = new WikiLink(match.Value);
            wikiLink.Target = WikiFreediaUtils.AsTopicName(wikiLink.Target);
            
            var fullMatch = match.Value;
            wikiLine = wikiLine.Replace(fullMatch, wikiLink.ToString());
        }
        
        // Bold text
        var boldPattern = "\\'\\'\\'[\\s\\S]*?\\'\\'\\'";
        foreach (Match match in Regex.Matches(wikiLine, boldPattern, RegexOptions.IgnoreCase).Reverse())
        {
            var fullMatch = match.Value;
            var matchContent = fullMatch[3..(fullMatch.Length - 3)];
            wikiLine = wikiLine.Replace(fullMatch, $"**{matchContent}**");
        }

        return wikiLine;
    }
    
    
}