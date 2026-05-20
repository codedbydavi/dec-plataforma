using Frontend.Models.DTOs;
using Frontend.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace Frontend.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel registerModel);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("token/", loginDto, _jsonOptions);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenResponseDto>(_jsonOptions);
            }

            return null;
        }

        public async Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel registerModel)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            
            var payload = new 
            {
                username = registerModel.Username,
                email = registerModel.Email,
                password = registerModel.Password,
                name = registerModel.Name
            };

            var response = await client.PostAsJsonAsync("register/", payload, _jsonOptions);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, errorContent);
        }
    }
}
