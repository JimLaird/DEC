using DEC.Shared.Services;
using DEC.Web.Components;
using DEC.Web.Services;
using Firebase.Database;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();
builder.Services.AddFluentUIComponents();

// Add device-specific services used by the DEC.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddSingleton(new FirebaseClient("https://decouriers-6eeae-default-rtdb.europe-west1.firebasedatabase.app"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(DEC.Shared._Imports).Assembly);

app.Run();
