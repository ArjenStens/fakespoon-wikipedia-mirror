using FakeSpoon.Lib.Cqe.Base;
using FakeSpoon.Lib.NostrClient.Models;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class SendEventToRelaysCommand : ICommand
{
    required public NostrEvent Event { get; set; }
}

public class SendEventToRelaysCommandHandler(
    ILogger<SendEventToRelaysCommandHandler> Logger
    ) : ICommandHandler<SendEventToRelaysCommand>
{
    public async Task Execute(SendEventToRelaysCommand cmd)
    {
        
    }
}