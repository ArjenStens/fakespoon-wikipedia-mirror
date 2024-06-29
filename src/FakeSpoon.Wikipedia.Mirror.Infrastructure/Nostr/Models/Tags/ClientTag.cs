using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

public class ClientTag : INostrTag
{
    public ClientTag(string clientName, PublicKey clientAddress, string? identifier, RelayAddress? relayHint)
    {
        ClientName = clientName;
        RelayHint = relayHint;
        Identifier = identifier;
    }

    public string Name { get; } = "client";

    public string ClientName { get; set; }
    
    public PublicKey ClientAddress { get; set; }
    
    public string Identifier { get; set; }
    
    public RelayAddress? RelayHint { get; set; }

    
    public string[] Rendered => new[] { Name, $"{ClientName}:{ClientAddress}:{Identifier}", RelayHint?.Value ?? "" };

}