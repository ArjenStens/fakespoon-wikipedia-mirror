namespace FakeSpoon.Lib.NostrClient.Models;

public abstract record NoteValue
{
    public abstract bool Validate(bool throwOnInvalid);
}