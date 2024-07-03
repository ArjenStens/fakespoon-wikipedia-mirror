namespace FakeSpoon.Lib.NostrClient.Models.Values;

public record RelayAddress : NoteValue
{
    public RelayAddress(string value)
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