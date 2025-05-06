using Blazored.LocalStorage;
using DEC.Shared.Models;
using Firebase.Auth;
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

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

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

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("userAuth");

            if (_firebaseAuthClient?.User != null)
            {
                _firebaseAuthClient.SignOut();
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        
        
    }
}
