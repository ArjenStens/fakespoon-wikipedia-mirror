

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class PublishedAtTag : INostrTag
{
    public PublishedAtTag(DateTime publishedAt)
    {
        PublishedAt = publishedAt;
    }

    public string Name { get; } = "p";

    public DateTime PublishedAt { get; set; }
    
    public string[] Rendered => new[] { Name, ((DateTimeOffset)PublishedAt).ToUnixTimeSeconds().ToString() };

}