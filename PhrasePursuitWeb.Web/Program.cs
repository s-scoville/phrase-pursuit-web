using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PhrasePursuitWeb.Web;
using PhrasePursuitWeb.Core.Interfaces;
using PhrasePursuitWeb.Core.Managers;
using PhrasePursuitWeb.Web.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(
    sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IStorageService, BrowserStorageService>();
builder.Services.AddScoped<StatisticsManager>();
builder.Services.AddScoped<SpinManager>();

await builder.Build().RunAsync();
