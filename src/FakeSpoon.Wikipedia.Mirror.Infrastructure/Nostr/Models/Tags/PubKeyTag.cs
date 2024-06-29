using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class PubKeyTag : INostrTag
{
    public PubKeyTag(PublicKey pubKey, RelayAddress? relayHint)
    {
        PubKey = pubKey;
        RelayHint = relayHint;
    }

    public string Name { get; } = "p";

    public PublicKey PubKey { get; set; }
    
    public RelayAddress? RelayHint { get; set; }

    
    public string[] Rendered => new[] { Name, PubKey.Value, RelayHint?.Value ?? "" };

}