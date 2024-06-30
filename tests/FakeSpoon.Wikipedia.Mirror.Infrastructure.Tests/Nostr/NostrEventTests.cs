
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Keys;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;
using FluentAssertions;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Tests.Nostr;

public class NostrEventTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void Should_SerializeEvent()
    {
        // arrange
        var evnt = new NostrEvent()
        {
            Kind = Kind.LongFormContent,
            PubKey = PublicKey.FromBech32("npub1w2fkh2057g032clzu5qp4t0xen36estz5nve4q3ayvwuvsdeuw6qn80sxn"),
            Tags = new INostrTag[]
            {
                new ClientTag("name", new PublicKeyValue("addr"), "identifier", new("relayHint")),
                new IdentifierTag("identifier")
            },
            CreatedAt = DateTime.Parse("1990-01-01T00:00:00+00:00"),
            Content = "empty",
        };
        var expected = "801b190d98688b0c14ecc7997c17a32f85191ef6498bd227fb1cd66fa3feae82";
        
        // act
        var actual = evnt.Id;

        // assert
        actual.Should().Be(expected);
    }
}