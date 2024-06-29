namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class IdentifierTag: INostrTag
{
    public IdentifierTag(){}
    
    public IdentifierTag(string identifier)
    {
        Value = identifier;
    }

    

    public string Name => StaticTagName;
    public static string StaticTagName => "d";

    public string Value { get; init; }
    
    public string[] Rendered => new[] { Name, Value };
    
}