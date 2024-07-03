using FakeSpoon.Lib.NostrClient.Models;
using FakeSpoon.Lib.NostrClient.Relay.Messages;
using FakeSpoon.Lib.NostrClient.Relay.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
    private readonly JsonSerializerSettings _jsonSettings;

    public SingleRelayClient(IRelayWebsocketClient relayWebsocketClient, ILogger<SingleRelayClient>? logger)
    {
        _logger = logger ?? new NullLogger<SingleRelayClient>();
        RelayWebsocketClient = relayWebsocketClient;
        _messageReceivedSubscription = RelayWebsocketClient.MessageReceived.Subscribe(HandleMessage);

        // cache settings, avoid getting new instance all the time
        _jsonSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Converters = new List<JsonConverter>
            {
                new UnixDateTimeConverter()
            },
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    public void Dispose()
    {
        _messageReceivedSubscription?.Dispose();
    }

    /// <summary>
    /// Underlying relay websocket client
    /// </summary>
    public IRelayWebsocketClient RelayWebsocketClient { get; }

    /// <summary>
    /// Provided message streams
    /// </summary>
    public RelayMessageStreams MessageStreams { get; } = new();

    /// <summary>
    /// Serializes request and sends message via websocket communicator. 
    /// It logs and re-throws every exception. 
    /// </summary>
    /// <param name="request">Request/message to be sent</param>
    public void Send<T>(T request)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var serialized = JsonConvert.SerializeObject(request, _jsonSettings);
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
        var parsed = JsonConvert.DeserializeObject<JArray>(formatted, _jsonSettings) ??
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

        var messageTypeString = messageTypeToken.ToString()?.ToUpperInvariant();

        try
        {
            var messageType = messageTypeString.ToRelayMessageType();
            switch (messageType)
            {
                case RelayMessageType.Event:
                    MessageStreams.EventSubject.OnNext(Deserialize<EventMessage>(formatted));
                    return;
                case RelayMessageType.EndOfStoredEvents:
                    MessageStreams.EoseSubject.OnNext(Deserialize<EndOfStoredEventsMessage>(formatted));
                    return;
                case RelayMessageType.Notice:
                    MessageStreams.NoticeSubject.OnNext(Deserialize<NoticeMessage>(formatted));
                    return;
                case RelayMessageType.Ok:
                    MessageStreams.OkSubject.OnNext(Deserialize<OkMessage>(formatted));
                    return;
                default:
                    _logger.LogWarning("Received unsupported message type {} relay", messageType);
                    return;
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Unable to deserialize message from relay {}, Message: {}", RelayWebsocketClient.Name, originalMessage);
        }
    }

    private T Deserialize<T>(string content) where T : RelayMessage
    {
        var deserialized = JsonConvert.DeserializeObject<T>(content, _jsonSettings) ??
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