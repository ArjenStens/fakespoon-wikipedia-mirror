using FakeSpoon.Lib.NostrClient.Events;
using FakeSpoon.Lib.NostrClient.Events.Tags;
using FakeSpoon.Lib.NostrClient.Events.Values;
using FakeSpoon.Lib.NostrClient.Extensions;
using FakeSpoon.Lib.NostrClient.Keys;
using FluentAssertions;

namespace FakeSpoon.Wikipedia.Mirror.Infrastructure.Tests.Nostr;

public class NostrEventTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void GetId_Should_ProduceValidId()
    {
        // arrange
        var evnt = new NostrEvent
        {
            Kind = Kind.LongFormContent,
            PubKey = PublicKey.FromHex("522075274c6883c150882b17931041095230e7a9b1c429e23d77571901d1ba9a"),
            Tags = new INostrTag[]
            {
                new ClientTag("name", new PublicKeyValue("addr"), "identifier", new("relayHint")),
                new IdentifierTag("identifier")
            },
            CreatedAt = DateTime.Parse("1990-01-01T00:00:00+00:00"),
            Content = "empty",
        };
        var expected = "b65e3f746724e67f9ed272618c771b78b202a10722180ddc203e2e620eeb724a";
        
        // act
        var actual = evnt.Id;

        // assert
        actual.Should().Be(expected);
    }

    [Test]
    public void SignEvent_Should_ProduceValidSignature()
    {
        var evnt = new NostrEvent
        {
            Kind = Kind.LongFormContent,
            PubKey = PublicKey.FromHex("522075274c6883c150882b17931041095230e7a9b1c429e23d77571901d1ba9a"),
            Tags = new INostrTag[]
            {
                new ClientTag("name", new PublicKeyValue("addr"), "identifier", new("relayHint")),
                new IdentifierTag("identifier")
            },
            CreatedAt = DateTime.Parse("1990-01-01T00:00:00+00:00"),
            Content = "empty",
        };
        var privateKey = Privatekey.FromBech32("nsec1cmh8cayum3dz6tvv0lwdnf0pdhj3am7ht0xqrkjq4vv3ljs4umhsdrwdvw");
        
        // act
        evnt.Sign(privateKey);
        var hasValidSignature = evnt.HasValidSignature();

        // assert
        hasValidSignature.Should().BeTrue();
    }
}