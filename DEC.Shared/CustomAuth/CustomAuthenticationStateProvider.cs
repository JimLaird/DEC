using Blazored.LocalStorage;
using DEC.Shared.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.CustomAuth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IAccountManagement
    {
        private bool _authenticated = false;

        private readonly ClaimsPrincipal Unauthenticated = new(new ClaimsIdentity());

        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthenticationStateProvider(FirebaseAuthClient firebaseAuthClient, ILocalStorageService localStorageService)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;
            var user = Unauthenticated;

            try
            {
                var userInfo = await _localStorageService.GetItemAsync<UserAuth>("userAuth");
                if (userInfo != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.Info.DisplayName),
                        new Claim(ClaimTypes.Email, userInfo.Info.Email)
                    };

                    // Get roles and add to claims
                    // claims.Add(new(ClaimTypes.Role, role));

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
