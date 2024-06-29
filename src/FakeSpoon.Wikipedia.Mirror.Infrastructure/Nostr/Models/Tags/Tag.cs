namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public interface INostrTag
{
    public string Name { get; }
    public string[] Rendered { get; }
}