using Blazored.LocalStorage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Storage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons;
using DEC.Services;
using DEC.Shared.Services;
using DEC.Shared.Models;
using DEC.Shared.CustomAuth;
using Google.Apis.Auth.OAuth2;


namespace DEC
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the DEC.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());
            builder.Services.AddAuthorizationCore();

            // Add Firebase Auth configuration
            var config = new FirebaseAuthConfig
            {
                ApiKey = Constants.FirebaseApiKey,
                AuthDomain = Constants.FirebaseAuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            using var stream = FileSystem.OpenAppPackageFileAsync(Constants.GoogleServiceAccount).Result;
            using var reader = new StreamReader(stream);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(reader.ReadToEnd())
            });

            // Register FirebaseAuthClient
            builder.Services.AddSingleton(new FirebaseAuthClient(config));

            // Initialise Firebase Database
            builder.Services.AddSingleton(new FirebaseClient("https://decouriers-6eeae-default-rtdb.europe-west1.firebasedatabase.app/"));

            // Register Firebase Storage
            builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
