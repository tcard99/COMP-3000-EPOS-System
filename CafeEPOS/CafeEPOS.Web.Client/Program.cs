using CafeEPOS.Shared.Services;
using CafeEPOS.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the CafeEPOS.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddTransient<LocalStorageService, LocalStorageService>();

await builder.Build().RunAsync();
