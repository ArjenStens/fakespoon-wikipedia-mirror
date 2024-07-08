using System.Text.Json;
using System.Text.Json.Serialization;
using FakeSpoon.Lib.NostrClient.Keys;
using FakeSpoon.Wikipedia.Mirror.Domain.Options;
using FakeSpoon.Wikipedia.Mirror.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    })
    .AddCustomServices(builder);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// WikipediaModule.ne
app.UseHttpsRedirection();

app.AddCustomWebApplicationResources();

app.Run();