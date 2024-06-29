
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class ParameterizedReplaceableEventReferenceTag : INostrTag
{
    public ParameterizedReplaceableEventReferenceTag(EventId eventId, RelayAddress? relayHint)
    {
        EventId = eventId;
        RelayHint = relayHint;
    }

    public string Name { get; } = "a";

    public EventId EventId { get; set; }
    
    public RelayAddress? RelayHint { get; set; }

    
    public string[] Rendered => new[] { Name, EventId.Value, RelayHint?.Value ?? "" };

}