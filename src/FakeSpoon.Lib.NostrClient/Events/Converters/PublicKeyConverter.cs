using FakeSpoon.Lib.NostrClient.Keys;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FakeSpoon.Lib.NostrClient.Events.Converters;

public class PublicKeyConverter() : JsonConverter<PublicKey>
{
    public override void WriteJson(JsonWriter writer, PublicKey? value, JsonSerializer serializer)
    {
        writer.WriteRawValue(JsonConvert.SerializeObject(value.Hex));
    }

    public override PublicKey? ReadJson(JsonReader reader, Type objectType, PublicKey? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonException("PublicKey should be bech32 string."); 
        }
        
        return PublicKey.FromBech32(reader.ReadAsString());
    }
}