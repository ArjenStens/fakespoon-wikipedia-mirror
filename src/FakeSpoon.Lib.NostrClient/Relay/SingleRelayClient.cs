using FakeSpoon.Lib.NostrClient.Relay.Messages;
using FakeSpoon.Lib.NostrClient.Relay.Requests;
using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using FakeSpoon.Lib.NostrClient.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace FakeSpoon.Lib.NostrClient.Relay;

/// <summary>
/// Nostr client that connects to a single relay. 
/// Subscribe to `Streams` to handle received messages.
/// </summary>
public class SingleRelayClient : IRelayClient
{
    private readonly ILogger<SingleRelayClient> _logger;
    private readonly IDisposable? _messageReceivedSubscription;

    public SingleRelayClient(IRelayWebsocketClient relayWebsocketClient, ILogger<SingleRelayClient>? logger)
    {
        _logger = logger ?? new NullLogger<SingleRelayClient>();
        RelayWebsocketClient = relayWebsocketClient;
        _messageReceivedSubscription = RelayWebsocketClient.MessageReceived.Subscribe(HandleMessage);
    }

    public void Dispose()
    {
        _messageReceivedSubscription?.Dispose();
    }
    
    public IRelayWebsocketClient RelayWebsocketClient { get; }
    
    public RelayMessageStreams MessageStreams { get; } = new();
    
    public void Send(IRelayMessage request)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var serialized = JsonConvert.SerializeObject(request, SerializerSettings.Settings);
            RelayWebsocketClient.Send(serialized);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while sending message '{request}'. Error: {error}", request, e.Message);
            throw;
        }
    }

    private void HandleMessage(ResponseMessage message)
    {
        try
        {
            var formatted = (message.Text ?? string.Empty).Trim();
            if (formatted.StartsWith("["))
            {
                OnArrayMessage(formatted, message);
                return;
            }

            _logger.LogWarning("Message format does not adhere to protocol, discarding message: {}", message.Text);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception while parsing message: {}", message.Text);
        }
    }

    private void OnArrayMessage(string formatted, ResponseMessage originalMessage)
    {
        var parsed = JsonConvert.DeserializeObject<JArray>(formatted, SerializerSettings.Settings) ??
                     throw new InvalidOperationException("Deserialized initial array is null, cannot continue.");
        
        if (parsed.Count <= 0)
        {
            _logger.LogError("Unable to parse array from relay {}, Message: {}", RelayWebsocketClient.Name, originalMessage);
            return;
        }

        var messageTypeToken = parsed[0];
        if (messageTypeToken.Type != JTokenType.String)
        {
            _logger.LogError("Relay message array does not start with string element, Relay{} Message: {}", RelayWebsocketClient.Name, originalMessage);
            return;
        }

        var messageTypeString = messageTypeToken.ToString()!.ToUpperInvariant();

        try
        {
            // A bit of a hacky way to utilize the JsonConverter to go from string to enum
            var messageType = JsonConvert.DeserializeObject<RelayMessageType>($"\"{messageTypeString}\""); 
            switch (messageType)
            {
                case RelayMessageType.Event:
                    MessageStreams.EventSubject.OnNext(Deserialize<EventResponse>(formatted));
                    return;
                case RelayMessageType.EndOfStoredEvents:
                    MessageStreams.EoseSubject.OnNext(Deserialize<EndOfStoredEventsResponse>(formatted));
                    return;
                case RelayMessageType.Notice:
                    MessageStreams.NoticeSubject.OnNext(Deserialize<NoticeResponse>(formatted));
                    return;
                case RelayMessageType.Ok:
                    MessageStreams.OkSubject.OnNext(Deserialize<OkResponse>(formatted));
                    return;
                default:
                    _logger.LogWarning("Received unsupported message type {} relay", messageType);
                    return;
            }
        }
        catch (JsonSerializationException ex)
        {
            _logger.LogError(ex, "Unable to deserialize message from relay {}, Message: {}", RelayWebsocketClient.Name, originalMessage);
        }
    }

    private T Deserialize<T>(string content) where T : RelayResponse
    {
        var deserialized = JsonConvert.DeserializeObject<T>(content, SerializerSettings.Settings) ??
                           throw new InvalidOperationException("Deserialized message is null, cannot continue");
        deserialized.Source = RelayWebsocketClient;
        return deserialized;
    }

    private RawRelayMessage Raw(ResponseMessage message)
    {
        return new RawRelayMessage
        {
            Message = message,
            Source = RelayWebsocketClient
        };
    }
}