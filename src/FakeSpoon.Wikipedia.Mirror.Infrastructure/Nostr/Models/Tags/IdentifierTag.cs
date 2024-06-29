namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class IdentifierTag : INostrTag
{
    public IdentifierTag(string identifier)
    {
        Value = identifier;
    }

    public string Name { get; } = "d";

    public string Value { get; }
    
    public string[] Rendered => new[] { Name, Value };

}