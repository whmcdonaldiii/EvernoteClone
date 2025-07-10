using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EvernoteClone;
using EvernoteClone.Services;
using Radzen;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5059/") });

// Add Radzen services
builder.Services.AddRadzenComponents();

// Add LocalStorage for offline functionality
builder.Services.AddBlazoredLocalStorage();

// Add our services
builder.Services.AddScoped<INoteService, ApiNoteService>();
builder.Services.AddScoped<IToastService, ToastService>();

await builder.Build().RunAsync();
