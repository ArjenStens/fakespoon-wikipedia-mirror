using FakeSpoon.Lib.NostrClient.Events.Values;

namespace FakeSpoon.Lib.NostrClient.Events.Tags;

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

    
    public string[] ToArray() => new[] { Name, EventId.Value, RelayHint?.Value ?? "" };
    
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new IdentifierTag();
    }

}