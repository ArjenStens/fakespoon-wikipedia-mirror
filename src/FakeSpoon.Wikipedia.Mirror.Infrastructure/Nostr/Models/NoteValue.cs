namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;

public abstract record NoteValue
{
    public abstract bool Validate();
}