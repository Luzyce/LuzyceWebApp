﻿using System.Net;
using Luzyce.Core.Models.User;
using Microsoft.AspNetCore.Components;

namespace LuzyceWebApp.Services
{
    public class TokenRefreshService(HttpClient httpClient, TokenValidationService tokenValidationService, NavigationManager navigationManager, CustomAuthenticationStateProvider customAuthenticationStateProvider) : IAsyncDisposable
    {
        private Timer? _timer;

        public void Start()
        {
            _timer = new Timer(RefreshToken, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        }

        private async void RefreshToken(object? state)
        {
            if (!await tokenValidationService.IsTokenValid())
            {
                return;
            }
            
            var response = await httpClient.GetAsync("/api/login/refreshToken");
            
            if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                navigationManager.NavigateTo("/logout");
            }
            
            await customAuthenticationStateProvider.UpdateAuthenticationState(CustomAuthenticationStateProvider
                .DeserializeJsonString<LoginResponseDto>(await response.Content.ReadAsStringAsync()).Token);
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer != null)
            {
                await _timer.DisposeAsync();
            }
        }
    }
}