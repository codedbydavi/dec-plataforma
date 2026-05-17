using Frontend.Models.DTOs;

namespace Frontend.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(LoginDto loginDto);
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
    }
}
