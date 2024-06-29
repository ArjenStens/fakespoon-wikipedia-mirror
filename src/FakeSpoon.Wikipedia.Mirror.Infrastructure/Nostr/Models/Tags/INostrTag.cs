namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public interface INostrTag
{
    public static string StaticTagName { get; set; }
    public string Name { get; }
    public string[] Rendered { get; }
}