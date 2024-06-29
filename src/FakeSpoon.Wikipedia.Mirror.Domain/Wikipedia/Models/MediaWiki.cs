using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;

public class MediaWikiContainer
{
    public MediaWiki MediaWiki { get; set; }
}

public class MediaWiki
{
    [JsonProperty("page")]
    public Page[] Pages { get; set; }
}