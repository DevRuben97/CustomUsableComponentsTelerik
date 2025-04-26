using CustomUsableComponentsTelerik.Client.Services;
using CustomUsableComponentsTelerik.Client.Services.TourGuide;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<ITourGuideService, TourGuideService>();

builder.Services.AddTelerikBlazor();

await builder.Build().RunAsync();
