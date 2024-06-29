namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class TitleTag : INostrTag
{
    public TitleTag(){}
    
    public TitleTag(string title)
    {
        Value = title;
    }

    public string Name => StaticTagName;
    public static string StaticTagName => "title";
    public string Value { get; init; }
    
    public string[] Rendered => new[] { Name, Value };

}