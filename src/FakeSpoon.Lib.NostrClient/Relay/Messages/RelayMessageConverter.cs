using System.Collections.Concurrent;
using System.Reflection;
using FakeSpoon.Lib.NostrClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FakeSpoon.Lib.NostrClient.Relay.Messages;

[AttributeUsage(AttributeTargets.Property)]
public class RelayRequestIndexAttribute : Attribute
{

    public int Index { get; }
    
    public RelayRequestIndexAttribute(int index)
    {
        Index = index;
    }
}

public class RelayMessageConverter() : JsonConverter<IRelayMessage>
{
    
    private static readonly ConcurrentDictionary<(MemberInfo, Type), Attribute?> AttributeByMemberInfoAndTypeCache = new();
    
    private static T? GetCustomAttribute<T>(MemberInfo memberInfo) where T : Attribute =>
        (T?)AttributeByMemberInfoAndTypeCache.GetOrAdd((memberInfo, typeof(T)), _ => memberInfo.GetCustomAttribute(typeof(T)));
    
    public override void WriteJson(JsonWriter writer, IRelayMessage relayMessage, JsonSerializer serializer)
    {
        var props = relayMessage.GetType().GetProperties();
        var orderedProperties = props.OrderBy(p => GetCustomAttribute<RelayRequestIndexAttribute>(p)?.Index);
        
        var array = new List<object>
        {
        };

        foreach (var property in orderedProperties)
        {
            var propertyValue = property.GetValue(relayMessage);
            try
            {
                array.Add(propertyValue);
            }
            catch (JsonWriterException ex)
            {
                throw new JsonWriterException($"Unable to serialize property {property.Name}", ex);
            }
        }
        
        writer.WriteRawValue(JsonConvert.SerializeObject(array, SerializerSettings.Settings));
    }

    public override IRelayMessage? ReadJson(JsonReader reader, Type objectType, IRelayMessage? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartArray)
        {
            throw new JsonException("RelayMessage should be an array"); 
        }
        
        var relayMessageValues = JArray.Load(reader);

        var relayMessageProperties = objectType
            .GetProperties()
            .Where(p => GetCustomAttribute<RelayRequestIndexAttribute>(p) != null)
            .OrderBy(p => GetCustomAttribute<RelayRequestIndexAttribute>(p)?.Index)
            .ToArray();
        
        var relayMessageInstance = (IRelayMessage) Activator.CreateInstance(objectType)!;

        var propertiesValues = new Dictionary<PropertyInfo, object?>();
        for (int i = 0; i < relayMessageValues.Count; i++)
        {
            var messageValue = relayMessageValues[i];
            var propertyForThisValue = relayMessageProperties[i];
            
            var deserializedValue = serializer.Deserialize(messageValue.CreateReader(), propertyForThisValue.PropertyType);
            propertiesValues.Add(propertyForThisValue, deserializedValue);
        }
        
        foreach (var (property, value) in propertiesValues)
        {
            property.SetValue(relayMessageInstance, value);
        }

        return relayMessageInstance;
    }
}