

namespace FakeSpoon.Lib.NostrClient.Events.Tags;

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
    
    public string[] ToArray() => new[] { Name, ((DateTimeOffset)PublishedAt).ToUnixTimeSeconds().ToString() };
    
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new PublishedAtTag();
    }
}