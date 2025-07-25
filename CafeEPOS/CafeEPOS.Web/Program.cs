using CafeEPOS.Shared.Services;
using CafeEPOS.Web.Components;
using CafeEPOS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    //.AddInteractiveWebAssemblyComponents();

// Add device-specific services used by the CafeEPOS.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddTransient<LocalStorageService, LocalStorageService>();

builder.Services.AddEposServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    //.AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(CafeEPOS.Shared._Imports).Assembly,
        typeof(CafeEPOS.Web.Client._Imports).Assembly);

app.Run();
