using EcommerceCompany.Infrastructure.Helpers.Extensions;
using EcommerceCompany.Receiver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddInjectedDependencies();
builder.Services.AddHostedService<OrderCreateReceiver>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
