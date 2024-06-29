

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class PublishedAtTag: INostrTag
{
    public PublishedAtTag(){}
    
    public PublishedAtTag(DateTime publishedAt)
    {
        PublishedAt = publishedAt;
    }
    
    public string Name => StaticTagName;
    public static string StaticTagName => "published_at";
    
    public DateTime PublishedAt { get; init; }
    
    public string[] Rendered => new[] { Name, ((DateTimeOffset)PublishedAt).ToUnixTimeSeconds().ToString() };
}