namespace FakeSpoon.Lib.NostrClient.Events;

public abstract record NoteValue
{
    public abstract bool Validate(bool throwOnInvalid);
}