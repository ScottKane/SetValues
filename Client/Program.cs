using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using ProtoBuf.Grpc.Client;
using SetValues.Client;
using SetValues.Contracts.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices(
    configuration =>
    {
        configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
        configuration.SnackbarConfiguration.HideTransitionDuration = 100;
        configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
        configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
        configuration.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        configuration.SnackbarConfiguration.ShowCloseIcon = false;
    });

builder.Services.AddSingleton(_ =>
{
    var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
    var options = new GrpcChannelOptions
    {
        HttpHandler = handler
    };

    return GrpcChannel.ForAddress("https://localhost:5001", options);
});

var provider = builder.Services.BuildServiceProvider();
var channel = provider.GetRequiredService<GrpcChannel>();

builder.Services.AddSingleton(channel.CreateGrpcService<ICustomerService>());

await builder.Build().RunAsync();
await channel.ShutdownAsync();