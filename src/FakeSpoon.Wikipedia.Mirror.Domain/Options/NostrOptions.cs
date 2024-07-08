namespace FakeSpoon.Wikipedia.Mirror.Domain.Options;

public class NostrOptions
{
    public string Nsec { get; set; }
    public IEnumerable<string> Relays { get; set; }
}