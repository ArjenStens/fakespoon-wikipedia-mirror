using FakeSpoon.Lib.NostrClient.Keys;
using FakeSpoon.Lib.NostrClient.Models;

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
        return evnt.PubKey.IsHexSignatureValid(evnt.Sig, evnt.Id);
    }
}