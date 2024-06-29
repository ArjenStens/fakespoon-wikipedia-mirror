using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class PubKeyTag: INostrTag
{
    public PubKeyTag(){}
    
    public PubKeyTag(PublicKey pubKey, RelayAddress? relayHint)
    {
        PubKey = pubKey;
        RelayHint = relayHint;
    }

    public string Name => StaticTagName;
    public static string StaticTagName => "p";

    public PublicKey PubKey { get; init; }
    
    public RelayAddress? RelayHint { get; init; }

    
    public string[] ToArray => new[] { Name, PubKey.Value, RelayHint?.Value ?? "" };
    
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new PubKeyTag();
    }
}