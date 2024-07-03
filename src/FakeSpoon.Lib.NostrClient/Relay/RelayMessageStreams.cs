using System.Reactive.Linq;
using System.Reactive.Subjects;
using FakeSpoon.Lib.NostrClient.Relay.Messages;

namespace FakeSpoon.Lib.NostrClient.Relay;

public class RelayMessageStreams
{
    internal readonly Subject<EventMessage> EventSubject = new();
    internal readonly Subject<NoticeMessage> NoticeSubject = new();
    internal readonly Subject<EndOfStoredEventsMessage> EoseSubject = new();
    internal readonly Subject<OkMessage> OkSubject = new();
    
    internal readonly Subject<IRelayMessage> UnknownMessageSubject = new();
    // internal readonly Subject<RawRelayMessage> UnknownRawSubject = new();

    /// <summary>
    /// Requested Nostr events
    /// </summary>
    public IObservable<EventMessage> EventStream => EventSubject.AsObservable();

    /// <summary>
    /// Human-readable messages
    /// </summary>
    public IObservable<NoticeMessage> NoticeStream => NoticeSubject.AsObservable();
    
    /// <summary>
    /// Information that all stored events have been sent out
    /// </summary>
    public IObservable<EndOfStoredEventsMessage> EoseStream => EoseSubject.AsObservable();
    
    /// <summary>
    /// Information if the sent event was accepted or rejected
    /// </summary>
    public IObservable<OkMessage> OkStream => OkSubject.AsObservable();

}