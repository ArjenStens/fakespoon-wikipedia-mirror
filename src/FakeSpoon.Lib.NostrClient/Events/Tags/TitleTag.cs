namespace FakeSpoon.Lib.NostrClient.Events.Tags;

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
    
    public string[] ToArray() => new[] { Name, Value };
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new TitleTag();
    }
}