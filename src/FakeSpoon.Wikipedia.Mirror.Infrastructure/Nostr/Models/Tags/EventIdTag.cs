using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class EventIdTag: INostrTag
{

    public EventIdTag(){}
    
    public EventIdTag(EventId eventId, RelayAddress? relayHint)
    {
        EventId = eventId;
        RelayHint = relayHint;
    }

    public string Name => StaticTagName;
    public static string StaticTagName => "e";

    public EventId EventId { get; init; }
    
    public RelayAddress? RelayHint { get; init; }

    
    public string[] Rendered => new[] { Name, EventId.Value, RelayHint?.Value ?? "" };

}