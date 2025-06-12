# DEC

This is my first project with Maui Blazor Hybrid. It is a management system for a friend of mine who runs a courier business in the UK.  
This app will run on **Windows**, **iOS**, and **Android**, and is intended to help him manage the day-to-day business.

---

## Setup Instructions

To run this project, you need to create a `Constants.cs` file in the root of your project with the following properties:

```csharp
public const string FirebaseApiKey = "YOUR_PROJECT_API_KEY";
public const string FirebaseAuthDomain = "YOUR_PROJECT_AUTH_DOMAIN";
public const string FirebaseProjectId = "YOUR_PROJECT_ID";
public const string GoogleServiceAccount = "YOUR_PROJECT_SERVICE_ACCOUNT_JSON_FILE";
public const string FirebaseStorageBucket = "YOUR_PROJECT_STORAGE_BUCKET";
public const string FirebaseDatabaseUrl = "YOUR_REALTIMEDATABASE_URL";
public const string GoogleApiUrl = "YOUR_GOOGLE_AUTH_API_URL";
```

Replace these values with your own from the Firebase console.

You also need to download the firebase authentication service account json file from the firebase console and store it in Resources/Raw


---

## NuGet Packages Used

The following NuGet packages are used in this project:

- **[Blazored.LocalStorage](https://www.nuget.org/packages/Blazored.LocalStorage)** Version: `4.5.0`
- **[FirebaseAdmin](https://www.nuget.org/packages/FirebaseAdmin)** Version: `3.2.0`
- **[FirebaseAuthentication.net](https://www.nuget.org/packages/FirebaseAuthentication.net)** Version: `4.1.0`
- **[FirebaseDatabase.net](https://www.nuget.org/packages/FirebaseDatabase.net)** Version: `5.0.0`
- **[Microsoft.AspNetCore.Components.Authorization](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Authorization)** Version: `9.0.4`
- **[Microsoft.AspNetCore.Components.QuickGrid](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.QuickGrid)** Version: `9.0.4`
- **[Microsoft.AspNetCore.Components.Web](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Web)** Version: `9.0.4`
- **[Microsoft.Maui.Controls](https://www.nuget.org/packages/Microsoft.Maui.Controls)** Version: `9.0.60`
- **[Microsoft.AspNetCore.Components.WebView.Maui](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebView.Maui)** Version: `9.0.60`
- **[Microsoft.Extensions.Logging.Debug](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug)** Version: `9.0.4`
- **[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)** Version: `13.0.3`
- **[FirebaseStorage.net](https://www.nuget.org/packages/FirebaseStorage.net)** Version: `1.0.3`
- **[iText7](https://www.nuget.org/packages/iText7)** Version: `9.2.0`
- **[iText7.Bouncy.Castle](https://www.nuget.org/[ackages/iText7.Bouncy.Castle.Adapter)** Version: `9.2.0`
 
---

## Features

- Cross-platform support for **Windows**, **iOS**, and **Android**.
- Firebase integration for authentication, database, and other functionalities.
- Blazor Hybrid framework for building modern and interactive UIs.
- PDF Generation on the fly.

---

## License

This is licensed using The Unlicense.  Do what you want with it.

---

## Acknowledgements

Special thanks to my friend Andrew Heron for inspiring this project.
```

