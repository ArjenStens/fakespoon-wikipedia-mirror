namespace FakeSpoon.Lib.NostrClient.Models.Tags;

public interface INostrTag
{
    public static string StaticTagName =>
        throw new NotImplementedException("Derrived types MUST implement the StaticTagName string");
    public string Name { get; }
    public string[] ToArray();

    public static INostrTag FromArray(IEnumerable<string> tagArray) => 
        throw new NotImplementedException("Derrived types MUST implement the FromArray method");
}