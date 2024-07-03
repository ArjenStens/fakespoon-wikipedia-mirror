using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Carter;
using FakeSpoon.Lib.Cqe.Base;
using FakeSpoon.Wikipedia.Mirror.Domain.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace FakeSpoon.Wikipedia.Mirror.Application;

public class WikipediaModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"api/wikipedia/add-dump", async (
                [FromBody] AddDumpRequest request,
                [FromServices] ICommandHandler<PostWikipediaDumpCommand> handler,
                [FromServices] ILogger<WikipediaModule> logger
            ) =>
            {
                var xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(request.Dump);
                    await handler.Execute(new  (){WikipediaDump = xmlDoc} );
                }
                catch (XmlException ex)
                {
                    logger.LogError(ex, "Unable to parse XML");
                    return Results.BadRequest("Unable to parse, invalid XML");
                }
                
                return Results.Ok("got it");
            })
            .WithName("Add wikipedia page")
            .WithTags("Wikipedia")
            .WithOpenApi();
        //
        // app.MapPost("/xml-test", async (XDocumentModel model) =>
        // {
        //     // model.Document <- your passed xml Document
        //     return Results.Ok(new { Value = model.Document.ToString() });
        // })
        //     .Accepts<WikipediaDumpRequest>("application/xml");
    }
    
}

public class AddDumpRequest
{
    public string Dump { get; set; }
}
public class WikipediaDumpRequest
{
    private WikipediaSiteInfo SiteInfo { get; set; }
}

public class WikipediaSiteInfo
{
    private string SiteName { get; set; }
}

internal sealed class XDocumentModel
{
    public XDocumentModel(XDocument document) => Document = document;

    public XDocument Document { get; init; }

    public static async ValueTask<XDocumentModel?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!context.Request.HasXmlContentType())
            throw new BadHttpRequestException(
                message: "Request content type was not a recognized Xml content type.",
                StatusCodes.Status415UnsupportedMediaType);

        return new XDocumentModel(await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, CancellationToken.None));
    }
} 

internal static class HttpRequestXmlExtensions
{
    public static bool HasXmlContentType(this HttpRequest request)
        => request.Headers.TryGetValue("Content-Type", out var contentType)
           && string.Equals(contentType, "application/xml", StringComparison.InvariantCulture);
}