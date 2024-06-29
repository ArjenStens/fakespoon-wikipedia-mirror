using System.Reflection;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Converters
{
    public class NostrTagsConverter() : JsonConverter<IEnumerable<INostrTag>>
    {
        
        private static readonly Dictionary<string, Type> TagTypeMappings = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(INostrTag).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToDictionary(t => (string)t.GetProperty("StaticTagName").GetValue(null), t => t);
        
        public override void WriteJson(JsonWriter writer, IEnumerable<INostrTag>? tags, JsonSerializer serializer)
        {
            var tagArray = tags.Select(tag => tag.ToArray).ToList();
            JsonConvert.SerializeObject(tagArray);
        }

        public override IEnumerable<INostrTag>? ReadJson(JsonReader reader, Type objectType, IEnumerable<INostrTag>? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException(); 
            }
            
            var tagsArray = serializer.Deserialize<IEnumerable<IEnumerable<string>>>(reader);
            var tags = new List<INostrTag>();
            
            foreach (var tag in tagsArray)
            {
                var tagName = tag.First();
                    
                // instantiate
                if (!TagTypeMappings.TryGetValue(tagName, out Type tagType))
                {
                    Console.WriteLine("Unknown tag name: {0}", tagName);
                    continue;
                }
                
                var tagInstance = (INostrTag) tagType.GetMethod("FromArray").Invoke(null, new []{tag});
                tags.Add(tagInstance);
            }
            return tags;
        }
    }
}