namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

public record Signature : NoteValue
{
    public Signature(string value)
    {
        Value = value;
    }

    public string Value { get; init; }
    public override bool Validate()
    {
        return true;
    }
}