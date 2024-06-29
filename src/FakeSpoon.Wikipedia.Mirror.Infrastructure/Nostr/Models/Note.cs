using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;

public class Note
{
    public required Kind Kind { get; set; }
    
    public required IEnumerable<INostrTag> Tags { get; set; }
    
    public required string Content { get; set; }
    
}