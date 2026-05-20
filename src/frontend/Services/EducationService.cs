using Frontend.Models.DTOs;

namespace Frontend.Services
{
    public interface IEducationService
    {
        Task<List<ClassGroupDto>> GetMyClassesAsync();
        Task<ClassDetailsDto?> GetClassDetailsAsync(int id);
        Task<bool> CreateClassAsync(string name);
        Task<bool> JoinClassAsync(string joinCode);
    }

    public class EducationService : IEducationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EducationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ClassGroupDto>> GetMyClassesAsync()
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync("classes/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ClassGroupDto>>() ?? new List<ClassGroupDto>();
            }
            
            return new List<ClassGroupDto>();
        }

        public async Task<ClassDetailsDto?> GetClassDetailsAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync($"classes/{id}/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ClassDetailsDto>();
            }
            
            return null;
        }

        public async Task<bool> CreateClassAsync(string name)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("classes/", new { name });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> JoinClassAsync(string joinCode)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("enrollments/", new { join_code = joinCode });
            return response.IsSuccessStatusCode;
        }
    }
}
