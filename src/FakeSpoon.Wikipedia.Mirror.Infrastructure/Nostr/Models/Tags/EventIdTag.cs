using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class EventIdTag : INostrTag
{
    public EventIdTag(EventId eventId, RelayAddress? relayHint)
    {
        EventId = eventId;
        RelayHint = relayHint;
    }

    public string Name { get; } = "e";

    public EventId EventId { get; set; }
    
    public RelayAddress? RelayHint { get; set; }

    
    public string[] Rendered => new[] { Name, EventId.Value, RelayHint?.Value ?? "" };

}