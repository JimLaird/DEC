using Blazored.LocalStorage;
using DEC.Shared.Models;
using Firebase.Auth;
using Firebase.Auth.Requests;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DEC.Shared.CustomAuth
{
    /// <summary>
    /// Handles authentication state for Blazor applications using Firebase Authentication.
    /// This class bridges Firebase authentication with Blazor's AuthenticationStateProvider
    /// and implements account management functionality.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        private bool _authenticated = false;

        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        /// <summary>
        /// Maps Firebase custom attribute keys to role names used in claims.
        /// For example, "IsAdmin" attribute from Firebase becomes "Admin" role in claims.
        /// </summary>
        Dictionary<string, string> keyMap = new Dictionary<string, string>
        {
            { "IsAdmin", "Admin" },
            { "IsUser", "User" },
        };

        
        public CustomAuthenticationStateProvider(FirebaseAuthClient firebaseAuthClient, ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Validates the current authentication token with Google's API and creates claims
        /// based on user information including roles from custom attributes.
        /// If validation fails, returns an unauthenticated state.
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
            var user = Unauthenticated;

            try
            {
                var userInfo = await _localStorageService.GetItemAsync<UserAuth>("userAuth");

                //  Add http body to the request
                var body = new StringContent($"{{\"idToken\":\"{userInfo?.Credential?.IdToken}\"}}", Encoding.UTF8, "application/json");
                var userResponse = await  _httpClient.PostAsync(Cnst.GoogleApiUrl, body);
                userResponse.EnsureSuccessStatusCode();

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var localUserInfo = System.Text.Json.JsonSerializer.Deserialize<Models.UserInfo>(userJson, jsonSerializerOptions);

                // End http body and response
                if (localUserInfo != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, localUserInfo.users.FirstOrDefault().DisplayName),
                        new Claim(ClaimTypes.Email, localUserInfo.users.FirstOrDefault().Email),
                        //new Claim(ClaimTypes.Name, userInfo.Info.DisplayName),
                        //new Claim(ClaimTypes.Email, userInfo.Info.Email)
                    };

                    // Get roles and add to claims
                    if (localUserInfo?.users?.FirstOrDefault()?.CustomAttributes != null)
                    {
                        Dictionary<string, bool> settings = JsonConvert.DeserializeObject<Dictionary<string, bool>>
                            (localUserInfo.users.FirstOrDefault().CustomAttributes);

                        var trueSettings = settings
                        .Where(x => x.Value)
                        .Select(x => keyMap.ContainsKey(x.Key) ? keyMap[x.Key] : null)
                        .Where(role => role != null); // Ensure null values are filtered out

                        foreach (var role in trueSettings)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                    }
                    

                    var id = new ClaimsIdentity(claims, nameof(CustomAuthenticationStateProvider));
                    user = new ClaimsPrincipal(id);
                    _authenticated = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Convenience method to check if a user is currently authenticated.
        /// </summary>
        /// <returns>True if user is authenticated, otherwise false</returns>
        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

        /// <summary>
        /// Creates a new user account in Firebase with the provided credentials.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <param name="username">User's display name</param>
        /// <returns>FormResult indicating success or containing error details</returns>
        public async Task<FormResult> RegisterAsync(string email, string password, string username)
        {
            string[] defaultDetail = ["An unknown error prevented registreation"];
            try
            {
                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password, username);
                return new FormResult
                {
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new FormResult
            {
                Succeeded = false,
                ErrorList = defaultDetail
            };

        }

        /// <summary>
        /// Authenticates a user with Firebase and stores the authentication token.
        /// On successful login, the authentication state is updated.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>FormResult indicating success or containing error details</returns>
        public async Task<FormResult> LoginAsync(string email, string password)
        {
            try
            {
                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                if (!string.IsNullOrWhiteSpace(result.User.Uid))
                {
                    await _localStorageService.SetItemAsync("userAuth", result.User);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new FormResult { Succeeded = true };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new FormResult
            {
                Succeeded = false,
                ErrorList = ["Invalid email and/or password"]
            };
        }

        /// <summary>
        /// Removes authentication data from local storage and signs out from Firebase.
        /// Updates the authentication state to reflect the logout.
        /// </summary>
        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("userAuth");

            if (_firebaseAuthClient?.User != null)
            {
                _firebaseAuthClient.SignOut();
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<UserAuth> GetUserAuthAsync()
        {
            return await _localStorageService.GetItemAsync<UserAuth>("userAuth");
        }

        public async Task<bool> IsTokenValidAsync()
        {
            var userAuth = await GetUserAuthAsync();
            if (userAuth?.Credential == null || string.IsNullOrEmpty(userAuth.Credential.IdToken))
            {
                return false;
            }

            try
            {
                // Try to validate the token - if it works, the token is valid
                var body = new StringContent($"{{\"idToken\":\"{userAuth.Credential.IdToken}\"}}", Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(Cnst.GoogleApiUrl, body);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                // If validation fails, token is likely expired
                return false;
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var userAuth = await GetUserAuthAsync();
                if (userAuth?.Credential == null || string.IsNullOrEmpty(userAuth.Credential.RefreshToken))
                {
                    return false;
                }

                // Call Firebase refresh token endpoint directly
                var refreshTokenRequest = new
                {
                    grant_type = "refresh_token",
                    refresh_token = userAuth.Credential.RefreshToken
                };

                var refreshContent = new StringContent(
                    JsonConvert.SerializeObject(refreshTokenRequest),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    $"https://securetoken.googleapis.com/v1/token?key={Cnst.FirebaseApiKey}",
                    refreshContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonConvert.DeserializeObject<RefreshTokenResponse>(responseJson);

                    // Update the token in the user auth object
                    userAuth.Credential.IdToken = tokenData.IdToken;
                   

                    userAuth.Credential.ExpiresIn = int.Parse(tokenData.ExpiresIn);
              

                    // Save the updated user auth
                    await _localStorageService.SetItemAsync("userAuth", userAuth);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing token: {ex.Message}");
            }

            return false;
        }

        private class RefreshTokenResponse
        {
            [JsonProperty("id_token")]
            public string IdToken { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonProperty("expires_in")]
            public string ExpiresIn { get; set; }
        }

        /// <summary>
        /// Gets a valid Firebase authentication token, refreshing if necessary.
        /// </summary>
        /// <returns>A valid Firebase ID token or empty string if authentication fails</returns>
        public async Task<string> GetAuthTokenAsync()
        {
            try
            {
                // Get the user authentication from local storage
                var userAuth = await GetUserAuthAsync();

                if (userAuth?.Credential != null && !string.IsNullOrEmpty(userAuth.Credential.IdToken))
                {
                    // Calculate expiration time from Created + ExpiresIn (seconds)
                    var expirationTime = userAuth.Credential.Created.AddSeconds(userAuth.Credential.ExpiresIn);

                    // Check if token is about to expire (less than 5 minutes remaining)
                    if (expirationTime <= DateTime.UtcNow.AddMinutes(5))
                    {
                        // Token is about to expire, try to refresh it
                        bool refreshed = await RefreshTokenAsync();

                        if (refreshed)
                        {
                            // Get the fresh token after refresh
                            userAuth = await GetUserAuthAsync();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }

                    return userAuth.Credential.IdToken;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting auth token: {ex.Message}");
            }

            return string.Empty;
        }


    }
}
