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
    public Task Execute(PostWikipediaDumpCommand cmd)
    {
        Logger.LogInformation(cmd.WikipediaDump.Value);

        var pages = ExtractPages(cmd.WikipediaDump);


        var tasks = pages
            .Select(page => handler.Execute(new() { WikiPage = page }))
            .ToList();
        return Task.WhenAll(tasks);
    }

    private IEnumerable<WikiPage> ExtractPages(XmlDocument wikipediaDump)
    {
        var wiki = wikipediaDump.GetElementsByTagName("mediawiki").Item(0); 
        var wikiJson = JsonConvert.SerializeXmlNode(wiki);
        return JsonConvert.DeserializeObject<MediaWikiContainer>(wikiJson).MediaWiki.Pages;
    }
}