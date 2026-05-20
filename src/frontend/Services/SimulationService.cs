using Frontend.Models.DTOs;

namespace Frontend.Services
{
    public interface ISimulationService
    {
        Task<List<ScenarioDto>> GetMyScenariosAsync();
        Task<ScenarioDto?> GetScenarioDetailsAsync(int id);
        Task<bool> CreateScenarioAsync(string familyName, decimal initialBalance);
        Task<bool> AddEntryAsync(int scenarioId, string type, string category, decimal amount, int month, bool recurrence);
        Task<bool> AddObjectiveAsync(int scenarioId, string description, decimal targetValue, int termMonths);
    }

    public class SimulationService : ISimulationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SimulationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ScenarioDto>> GetMyScenariosAsync()
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync("scenarios/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ScenarioDto>>() ?? new List<ScenarioDto>();
            }
            
            return new List<ScenarioDto>();
        }

        public async Task<ScenarioDto?> GetScenarioDetailsAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.GetAsync($"scenarios/{id}/");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ScenarioDto>();
            }
            
            return null;
        }

        public async Task<bool> CreateScenarioAsync(string familyName, decimal initialBalance)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("scenarios/", new 
            { 
                family_name = familyName, 
                initial_balance = initialBalance 
            });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddEntryAsync(int scenarioId, string type, string category, decimal amount, int month, bool recurrence)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("entries/", new 
            { 
                scenario = scenarioId,
                type = type,
                category = category,
                amount = amount,
                month = month,
                recurrence = recurrence
            });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddObjectiveAsync(int scenarioId, string description, decimal targetValue, int termMonths)
        {
            var client = _httpClientFactory.CreateClient("DecApi");
            var response = await client.PostAsJsonAsync("objectives/", new 
            { 
                scenario = scenarioId,
                description = description,
                target_value = targetValue,
                term_months = termMonths
            });
            return response.IsSuccessStatusCode;
        }
    }
}
