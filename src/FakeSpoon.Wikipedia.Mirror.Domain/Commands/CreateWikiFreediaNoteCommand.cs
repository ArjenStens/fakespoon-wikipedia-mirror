using FakeSpoon.Wikipedia.Mirror.Domain.Nostr.Models.Tags;
using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class CreateWikiFreediaNoteCommand : ICommand
{
    required public WikiPage WikiPage { get; set; }
}

public class CreateWikiFreediaNoteCommandHandler(
    ILogger<CreateWikiFreediaNoteCommandHandler> Logger) : ICommandHandler<CreateWikiFreediaNoteCommand>
{
    public Task Execute(CreateWikiFreediaNoteCommand cmd)
    {
        Logger.LogInformation("page");

        

        var categories = GetCategories(cmd.WikiPage.Revision.Text.Value);
        var markdownContent = GetMarkdownContent(cmd.WikiPage.Revision.Text.Value);
        var note = new Note
        {
            Kind = Kind.LongFormContent,
            Tags = new INostrTag[]
            {
                new IdentifierTag($"wiki-{cmd.WikiPage.Id}"),
                new TitleTag(cmd.WikiPage.Title),
                new CategoriesTag(categories)
            },
            Content = null
        };
        
        return Task.CompletedTask;
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
    
    public IEnumerable<string> GetMarkdownContent(string wikiText)
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
            
            markdownLines.Add(currentLine);
        }
        
        return markdownLines;
    }
}