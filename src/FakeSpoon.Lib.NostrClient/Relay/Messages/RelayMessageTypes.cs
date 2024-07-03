namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

public enum RelayMessageType
{
    Request,
    Event,
    Notice,
    EndOfStoredEvents ,
    Ok ,
    Close,
        
    Unknown,
}
public static class RelayMessageTypesExtensions
{
    public static string ToString(RelayMessageType type) => type switch
    {
        RelayMessageType.Request    => "REQ",
        RelayMessageType.Ok    => "OK",
        RelayMessageType.Event    => "EVeNT",
        RelayMessageType.EndOfStoredEvents    => "EOSE",
        RelayMessageType.Close    => "CLOSE",
        RelayMessageType.Notice    => "NOTICE",
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected type value: {type}"),
    };
    
    public static RelayMessageType ToRelayMessageType(this string? type) => type switch
    {
       "REQ" => RelayMessageType.Request,
       "OK" => RelayMessageType.Ok, 
       "EVENT" => RelayMessageType.Event,
       "EOSE" => RelayMessageType.EndOfStoredEvents,
       "CLOSE" => RelayMessageType.Close, 
       "NOTICE"  => RelayMessageType.Notice,
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected RelayMessageType value: {type}"),
    };
}