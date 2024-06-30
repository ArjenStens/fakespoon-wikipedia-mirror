namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

public record PublicKeyValue : NoteValue
{
    public PublicKeyValue(string value)
    {
        // TODO: validate
        Value = value;
    }

    public string Value { get; init; }
    public override bool Validate()
    {
        return true;
    }
}