using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using gain.Data;
using System.Collections.Generic;
using System.Linq;

namespace OpenAITest.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUrl;
        private readonly ApplicationDbContext _dbContext;

        public OpenAIService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            _requestUrl = configuration["OpenAI:RequestUrl"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            _dbContext = dbContext;
        }
        
        public async Task<string> GenerateWorkoutAsync(int duration, uint userLevel, List<int> equipmentIds)
        {
            // We have to include some more info in here, like equipment and skill level
            // but that will be after all the user-info is correctly implemented.
            // For now it just generates based on time
            // We also need to remove the exercises that cannot be chosen from the request, but this will also come after the user-info is correctly implemented
            
            var exercises = _dbContext.Exercises.ToList();

            string amount;

            switch (duration)
            {
                case 5:
                    amount = "4-6";
                    break;
                case 10:
                    amount = "7-9";
                    break;
                case 15:
                    amount = "10-12";
                    break;
                case 30:
                    amount = "18-22";
                    break;
                default:
                    amount = "8-18";
                    break;
            }


            var exerciseList = 
                string.Join("\n", 
                exercises.Where(e =>
                    (e.Difficulty == userLevel) &&
                    (e.Equipment == null || equipmentIds.Contains(e.Equipment.Value)))
                .Select(e => 
                    $"{e.Id}: {e.Name}"));
            
            Console.WriteLine(exerciseList);
            
            
            string prompt = $"Generate a workout with {amount} exercises. " +
                            $"Make sure to include both strength exercises and stretching exercises. " +
                            $"Try to hit as many big muscle groups as possible. " +
                            $"Because your response will be parsed, I want you to ONLY respond with the IDs of the exercises, separated by ','. " +
                            $"Pick from the following exercises:\n" +
                            exerciseList;
            
            Console.WriteLine($"Generating workout with {amount} exercises");
            Console.WriteLine($"Prompt: {prompt}");
            
            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a workoutgenerator, and you should ONLY select the IDs of the exercises you chose"},
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseString);
            Console.WriteLine(result.choices[0].message.content);
            return result.choices[0].message.content;
        }
    }
}
