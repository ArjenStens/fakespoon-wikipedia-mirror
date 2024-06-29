namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class TitleTag : INostrTag
{
    public TitleTag(string title)
    {
        Value = title;
    }

    public string Name { get; } = "title";

    public string Value { get; }
    
    public string[] Rendered => new[] { Name, Value };

}