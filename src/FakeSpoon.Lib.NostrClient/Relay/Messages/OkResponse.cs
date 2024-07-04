using FakeSpoon.Lib.NostrClient.Relay.Requests;
using Newtonsoft.Json;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

[JsonConverter(typeof(RelayRequestConverter))]
public class OkResponse : RelayResponse
{
    [RelayRequestIndex(0)]
    public RelayMessageType MessageType { get; init; } = RelayMessageType.Ok;
    
    [RelayRequestIndex(1)]
    public string? EventId { get; init; }

    /// <summary>
    /// Returns true when the event was accepted and stored on the server (even for duplicates). 
    /// Returns false when the event was rejected and not stored.
    /// </summary>
    [RelayRequestIndex(2)]
    public bool Accepted { get; init; }

    /// <summary>
    /// Additional information as to why the command succeeded or failed.
    /// </summary>
    [RelayRequestIndex(3)]
    public string? Message { get; init; }
}