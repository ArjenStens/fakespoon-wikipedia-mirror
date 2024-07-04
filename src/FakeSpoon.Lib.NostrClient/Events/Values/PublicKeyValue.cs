namespace FakeSpoon.Lib.NostrClient.Events.Values;

public record PublicKeyValue : NoteValue
{
    public PublicKeyValue(string value)
    {
        Value = value;

        Validate(true);
    }

    public string Value { get; init; }

    public sealed override bool Validate(bool throwOnInvalid)
    {
        return true; // TODO: Add this
    }
}