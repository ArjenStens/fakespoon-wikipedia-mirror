using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Events.Tags;
using FakeSpoon.Lib.NostrClient.Events.Values;
using FluentAssertions;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Tests;

public class NostrTagDeserializerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Should_DeserializeTags()
    {
        // arrange
        var validEvent =
            "{\"tags\":[[\"d\",\"jill-stein\"],[\"client\",\"wikifreedia\",\"31990:fa984bd7dbb282f07e16e7ae87b26a2a7b9b90b7246a44771f0cf5ae58018f52:1716498133442\"],[\"t\",\"1\"],[\"title\",\"Jill Stein\"],[\"c\",\"People\"],[\"published_at\",\"1719658862\"]],\"kind\":30818}";
            // act
            
        var nostrTag = JsonConvert.DeserializeObject<NostrEvent>(validEvent);

        // assert
    
    }
    
    [Test]
    public void Should_SerializeTags()
    {
        // arrange
        var evnt = new NostrEvent
        {
            Kind = Kind.LongFormContent,
            Content = "empty",
            Tags = new INostrTag[]
            {
                new ClientTag("name", new PublicKeyValue("addr"), "identifier", new("relayHint")),
                new IdentifierTag("identifier")
            }
        };
        var expected = "";
        
        // act
        var actual = JsonSerializer.Serialize(evnt);

        // assert
        actual.Should().Be(expected);
    }
}