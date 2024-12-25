using SampleApiWithEureka.ServiceRegistery.Extensions;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromEureka();
builder.Services.AddDiscoveryClient();

var app = builder.Build();

app.MapReverseProxy();

app.Run();
