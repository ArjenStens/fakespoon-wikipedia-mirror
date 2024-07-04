using System.Reactive.Linq;
using System.Reactive.Subjects;
using FakeSpoon.Lib.NostrClient.Relay.Messages;
using FakeSpoon.Lib.NostrClient.Relay.Messages.Response;

namespace FakeSpoon.Lib.NostrClient.Relay;

public class RelayMessageStreams
{
    internal readonly Subject<EventResponse> EventSubject = new();
    internal readonly Subject<NoticeResponse> NoticeSubject = new();
    internal readonly Subject<EndOfStoredEventsResponse> EoseSubject = new();
    internal readonly Subject<OkResponse> OkSubject = new();
    
    internal readonly Subject<IRelayMessage> UnknownMessageSubject = new();
    public IObservable<EventResponse> EventStream => EventSubject.AsObservable();
    public IObservable<NoticeResponse> NoticeStream => NoticeSubject.AsObservable();
    public IObservable<EndOfStoredEventsResponse> EoseStream => EoseSubject.AsObservable();
    public IObservable<OkResponse> OkStream => OkSubject.AsObservable();

}