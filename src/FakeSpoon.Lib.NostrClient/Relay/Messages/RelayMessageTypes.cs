using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

[JsonConverter(typeof(StringEnumConverter))]
public enum RelayMessageType
{
    [EnumMember(Value = "REQ")]
    Request,
    
    [EnumMember(Value = "EVENT")]
    Event,
    
    [EnumMember(Value = "NOTICE")]
    Notice,
    
    [EnumMember(Value = "EOSE")]
    EndOfStoredEvents,
    
    [EnumMember(Value = "OK")]
    Ok,
    
    [EnumMember(Value = "CLOSE")]
    Close,
        
    [EnumMember(Value = "UNKNOWN")]
    Unknown
}