using Frontend.Models.DTOs;

namespace Frontend.Services
{
    public interface IEducationService
    {
        Task<List<TurmaDto>> GetMinhasTurmasAsync();
        Task<TurmaDetalhesDto?> GetTurmaDetalhesAsync(int id);
    }

    public class EducationService : IEducationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EducationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<TurmaDto>> GetMinhasTurmasAsync()
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync("turmas/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<TurmaDto>>() ?? new List<TurmaDto>();
            }
            
            return new List<TurmaDto>();
        }

        public async Task<TurmaDetalhesDto?> GetTurmaDetalhesAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync($"turmas/{id}/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TurmaDetalhesDto>();
            }
            
            return null;
        }
    }
}
