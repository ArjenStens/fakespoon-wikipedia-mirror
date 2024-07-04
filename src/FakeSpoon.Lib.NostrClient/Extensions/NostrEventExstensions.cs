using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Keys;

namespace FakeSpoon.Lib.NostrClient.Extensions;

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
        return evnt.Pubkey.IsHexSignatureValid(evnt.Sig, evnt.Id);
    }
}