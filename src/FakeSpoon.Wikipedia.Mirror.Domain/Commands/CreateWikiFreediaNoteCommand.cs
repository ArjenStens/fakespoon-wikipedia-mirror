using System.Xml;
using System.Xml.Serialization;
using FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;
using FakeSpoon.Wikipedia.Mirror.Infrastructure.Cqe.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Commands;

public class CreateWikiFreediaNoteCommand : ICommand
{
    required public Page Page { get; set; }
}

public class CreateWikiFreediaNoteCommandHandler(
    ILogger<CreateWikiFreediaNoteCommandHandler> Logger) : ICommandHandler<CreateWikiFreediaNoteCommand>
{
    public Task Execute(CreateWikiFreediaNoteCommand cmd)
    {
        Logger.LogInformation("page");
        
        return Task.CompletedTask;
    }
}