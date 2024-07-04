using System.Reactive.Linq;
using System.Reactive.Subjects;
using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay;

public class RelayMessageStreams
{
    internal readonly Subject<EventResponse> EventSubject = new();
    internal readonly Subject<NoticeResponse> NoticeSubject = new();
    internal readonly Subject<EndOfStoredEventsResponse> EoseSubject = new();
    internal readonly Subject<OkResponse> OkSubject = new();
    
    internal readonly Subject<IRelayMessage> UnknownMessageSubject = new();
    // internal readonly Subject<RawRelayMessage> UnknownRawSubject = new();

    /// <summary>
    /// Requested Nostr events
    /// </summary>
    public IObservable<EventResponse> EventStream => EventSubject.AsObservable();

    /// <summary>
    /// Human-readable messages
    /// </summary>
    public IObservable<NoticeResponse> NoticeStream => NoticeSubject.AsObservable();
    
    /// <summary>
    /// Information that all stored events have been sent out
    /// </summary>
    public IObservable<EndOfStoredEventsResponse> EoseStream => EoseSubject.AsObservable();
    
    /// <summary>
    /// Information if the sent event was accepted or rejected
    /// </summary>
    public IObservable<OkResponse> OkStream => OkSubject.AsObservable();

}