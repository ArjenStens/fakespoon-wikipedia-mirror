namespace FakeSpoon.Lib.NostrClient.Models.Values;

public record Signature : NoteValue
{
    public Signature(string value)
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