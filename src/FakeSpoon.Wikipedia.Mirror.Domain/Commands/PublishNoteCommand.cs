// using System.Xml;
// using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;
// using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;
// using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Keys;
// using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models;
// using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Values;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
//
// namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;
//
// public class PublishNoteCommand : ICommand
// {
//     required public NostrEvent NostrEvent { get; set; }
// }
//
// public class PublishNoteCommandHandler(
//     ILogger<PublishNoteCommandHandler> Logger) : ICommandHandler<PublishNoteCommand>
// {
//     public Task Execute(PublishNoteCommand cmd)
//     {
//         var privatekey = Privatekey.GenerateNew();
//         var pubkey = privatekey.DerivePublicKey();
//
//         var evnt = cmd.NostrEvent;
//         evnt.PubKey = pubkey;
//         
//         evnt.Sig = new Signature(privatekey)
//         
//     }
// }