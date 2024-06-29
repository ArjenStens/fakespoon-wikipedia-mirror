

using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Converters;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using PublicKey = FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Keys.PublicKey;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;

public class NostrEvent
{

    public string Id { get; set; }

    public PublicKey PubKey { get; set; }
    public required Kind Kind { get; set; }

    [JsonConverter(typeof(NostrTagsConverter))]
    public required IEnumerable<INostrTag> Tags { get; set; }

    public required string Content { get; set; }

    public Signature Sig { get; set; }

    // private byte[] ComputeIdBytes()
    // {
    //     var clone = DeepClone("<<leading_zero>>", null);
    //     var json = JsonConvert.SerializeObject(renderedEvent, ArraySettings);
    //
    //     // we need to include id=0 as a number instead of string
    //     json = ReplaceValue(json, "\"<<leading_zero>>\"", "0");
    //
    //     var hash = HashExtensions.GetSha256(json);
    //     return hash;
    // }

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