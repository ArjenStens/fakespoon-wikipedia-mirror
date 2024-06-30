using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class PubKeyTag: INostrTag
{
    public PubKeyTag(){}
    
    public PubKeyTag(PublicKeyValue pubKeyValue, RelayAddress? relayHint)
    {
        PubKeyValue = pubKeyValue;
        RelayHint = relayHint;
    }

    public string Name => StaticTagName;
    public static string StaticTagName => "p";

    public PublicKeyValue PubKeyValue { get; init; }
    
    public RelayAddress? RelayHint { get; init; }

    
    public string[] ToArray() => new[] { Name, PubKeyValue.Value, RelayHint?.Value ?? "" };
    
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new PubKeyTag();
    }
}