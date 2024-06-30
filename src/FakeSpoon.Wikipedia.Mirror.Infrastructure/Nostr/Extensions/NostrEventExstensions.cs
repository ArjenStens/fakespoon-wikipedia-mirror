using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Keys;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Utils;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Extensions;

public static class NostrEventExstensions
{
    public static NostrEvent Sign(this NostrEvent evnt, Privatekey privateKey)
    {
        var x = privateKey.SignHex(evnt.Id) ?? throw new InvalidOperationException();

        evnt.Sig = x;
        return evnt;
    }
    
    public static bool HasValidSignature(this NostrEvent evnt)
    {
        return evnt.PubKey.IsHexSignatureValid(evnt.Sig, evnt.Id);
    }
}