using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class ClientTag: INostrTag
{
    public ClientTag(string clientName, PublicKey clientAddress, string? identifier, RelayAddress? relayHint)
    {
        ClientName = clientName;
        RelayHint = relayHint;
        Identifier = identifier;
    }
    
    public ClientTag(){}

    public string Name => StaticTagName;
    public static string StaticTagName => "client";

    public string ClientName { get; init; }
    
    public PublicKey ClientAddress { get; init; }
    
    public string Identifier { get; init; }
    
    public RelayAddress? RelayHint { get; init; }
    
    public string[] ToArray => new[] { Name, $"{ClientName}:{ClientAddress}:{Identifier}", RelayHint?.Value ?? "" };
    
    public static INostrTag FromArray(IEnumerable<string> tagArray)
    {
        return new ClientTag();
    }
}