using System.Security.Cryptography;
using System.Text;
using FakeSpoon.Lib.NostrClient.Models.Converters;
using FakeSpoon.Lib.NostrClient.Models.Tags;
using FakeSpoon.Lib.NostrClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using PublicKey = FakeSpoon.Lib.NostrClient.Keys.PublicKey;

namespace FakeSpoon.Lib.NostrClient.Models;

public class NostrEvent
{
    public string Id => ComputeId();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PublicKey PubKey { get; set; }
    public required Kind Kind { get; set; }
    
    [Newtonsoft.Json.JsonConverter(typeof(NostrTagsConverter))]
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
        var json = JsonConvert.SerializeObject(array, ArraySettings);
        
        var stringBytes = Encoding.UTF8.GetBytes(json);
        var hashBytes = SHA256.HashData(stringBytes);
        var hashHex = hashBytes.ToHex();
        return hashHex;
    }
    
    public static JsonSerializerSettings Settings => new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        Converters = new List<JsonConverter>
        {
            new UnixDateTimeConverter()
        },
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    /// <summary>
    /// Unified JSON settings that serializes messages into array
    /// </summary>
    public static JsonSerializerSettings ArraySettings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        Converters = new List<JsonConverter>
        {
            new UnixDateTimeConverter()
        },
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
}