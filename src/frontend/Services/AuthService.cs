using Frontend.Models.DTOs;
using Frontend.Models;

namespace Frontend.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterViewModel registerModel);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("token/", loginDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
            }

            return null;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerModel)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("register/", new 
            {
                username = registerModel.Username,
                email = registerModel.Email,
                password = registerModel.Password,
                name = registerModel.Name
            });

            return response.IsSuccessStatusCode;
        }
    }
}
