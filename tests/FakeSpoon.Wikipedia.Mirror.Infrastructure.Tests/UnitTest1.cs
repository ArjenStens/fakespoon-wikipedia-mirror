using System.Diagnostics;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;
using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Tests;

public class Tests
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
}