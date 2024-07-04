using System.Collections.Concurrent;
using System.Reflection;
using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Events.Tags;
using FakeSpoon.Lib.NostrClient.Utils;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FakeSpoon.Lib.NostrClient.Relay.Requests;

[AttributeUsage(AttributeTargets.Property)]
public class RelayRequestIndexAttribute : Attribute
{

    public int Index { get; }
    
    public RelayRequestIndexAttribute(int index)
    {
        Index = index;
    }
}

public class RelayRequestConverter() : JsonConverter<IRelayRequest>
{
    
    private static readonly ConcurrentDictionary<(MemberInfo, Type), Attribute?> AttributeByMemberInfoAndTypeCache = new();
    
    private static T? GetCustomAttribute<T>(MemberInfo memberInfo) where T : Attribute =>
        (T?)AttributeByMemberInfoAndTypeCache.GetOrAdd((memberInfo, typeof(T)), _ => memberInfo.GetCustomAttribute(typeof(T)));
    
    public override void WriteJson(JsonWriter writer, IRelayRequest relayRequest, JsonSerializer serializer)
    {
        var props = relayRequest.GetType().GetProperties();
        var orderedProperties = props.OrderBy(p => GetCustomAttribute<RelayRequestIndexAttribute>(p)?.Index);
        
        var array = new List<object>()
        {
        };

        foreach (var property in orderedProperties)
        {
            var propertyValue = property.GetValue(relayRequest);
            try
            {
                array.Add(propertyValue);
            }
            catch (JsonWriterException ex)
            {
                throw new JsonWriterException($"Unable to serialize property {property.Name}", ex);
            }
        }
        
        writer.WriteRawValue(JsonConvert.SerializeObject(array));
    }

    public override IRelayRequest? ReadJson(JsonReader reader, Type objectType, IRelayRequest? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        // if (reader.TokenType != JsonToken.StartArray)
        // {
        //     throw new JsonException("Tags should be a 2-dimensional array"); 
        // }
        //     
        // var tagsArray = serializer.Deserialize<IEnumerable<IEnumerable<string>>>(reader);
        // var tags = new List<INostrTag>();
        //     
        // foreach (var tag in tagsArray)
        // {
        //     var tagName = tag.First();
        //         
        //     if (!TagTypeMappings.TryGetValue(tagName, out Type tagType))
        //     {
        //         Console.WriteLine("Unknown tag name: {0}", tagName);
        //         continue;
        //     }
        //         
        //     var tagInstance = (INostrTag) tagType.GetMethod("FromArray").Invoke(null, new []{tag});
        //     tags.Add(tagInstance);
        // }
        // return tags;
        return new PublishEventRequest(new (){Kind = Kind.LongFormContent, Content = "", Tags = new INostrTag[]{}});
    }
}