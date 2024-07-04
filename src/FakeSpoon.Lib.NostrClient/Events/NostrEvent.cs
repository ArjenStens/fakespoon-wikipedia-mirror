using System.Security.Cryptography;
using System.Text;
using FakeSpoon.Lib.NostrClient.Events.Converters;
using FakeSpoon.Lib.NostrClient.Events.Tags;
using FakeSpoon.Lib.NostrClient.Utils;
using Newtonsoft.Json;
using PublicKey = FakeSpoon.Lib.NostrClient.Keys.PublicKey;

namespace FakeSpoon.Lib.NostrClient.Events;

public class NostrEvent
{
    [JsonProperty]
    public string Id => ComputeId();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonConverter(typeof(PublicKeyConverter))]
    public PublicKey PubKey { get; set; }
    
    public required Kind Kind { get; set; }
    
    [JsonConverter(typeof(NostrTagsConverter))]
    public required IEnumerable<INostrTag> Tags { get; set; }

    public required string Content { get; set; }

    public string Sig { get; set; }
    
    private string ComputeId()
    {
        var array = new List<dynamic>()
        {
            0,
            PubKey.Hex,
            ((DateTimeOffset)CreatedAt).ToUnixTimeSeconds(),
            Kind,
            Tags.Select(t => t.ToArray()),
            Content
        };
        var json = JsonConvert.SerializeObject(array, SerializerSettings.Settings);
        
        var stringBytes = Encoding.UTF8.GetBytes(json);
        var hashBytes = SHA256.HashData(stringBytes);
        var hashHex = hashBytes.ToHex();
        return hashHex;
    }
}