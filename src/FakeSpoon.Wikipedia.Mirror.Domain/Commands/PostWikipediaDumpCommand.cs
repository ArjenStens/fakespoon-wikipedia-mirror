using System.Xml;
using FakeSpoon.Lib.Cqe.Base;
using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class PostWikipediaDumpCommand : ICommand
{
    required public XmlDocument WikipediaDump { get; set; }
}

public class PostWikipediaDumpCommandHandler(
    ILogger<PostWikipediaDumpCommandHandler> Logger,
    ICommandHandler<CreateWikiFreediaNoteCommand> handler) : ICommandHandler<PostWikipediaDumpCommand>
{
    public async Task Execute(PostWikipediaDumpCommand cmd)
    {
        Logger.LogInformation(cmd.WikipediaDump.Value);

        var wikiPages = ExtractPages(cmd.WikipediaDump);
        var pagesMinusRedirects = wikiPages.Where(p => !p.IsRedirect);

        foreach (var page in pagesMinusRedirects)
        {
            await ProcessPage(page);
        }
        
    }

    private async Task ProcessPage(WikiPage page)
    {
        try
        {
            await handler.Execute(new() { WikiPage = page });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing page {}", page.Title);
        }
    }

    private IEnumerable<WikiPage> ExtractPages(XmlDocument wikipediaDump)
    {
        var wiki = wikipediaDump.GetElementsByTagName("mediawiki").Item(0); 
        var wikiJson = JsonConvert.SerializeXmlNode(wiki);
        return JsonConvert.DeserializeObject<MediaWikiContainer>(wikiJson).MediaWiki.Pages;
    }
}