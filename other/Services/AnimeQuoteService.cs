using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VBotDiscord.other.Services {
    public class AnimeQuoteService {
        private readonly HttpClient _httpClient;

        public AnimeQuoteService() { 
            _httpClient = new HttpClient();
        }

        public async Task<(AnimeQuotes? Quote, string? Error)> GetRandomQuoteAsync(string animeCharacter) {
            try {
                var response = await _httpClient.GetAsync($"https://yurippe.vercel.app/api/quotes?character={animeCharacter}&random=1");
                
                if (response.StatusCode == HttpStatusCode.NotFound) {
                    return (null, $"Cannot find the quote for {animeCharacter}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var quotes = JsonSerializer.Deserialize<List<AnimeQuotes>>(json);

                return (quotes?.FirstOrDefault(), null);

            } catch (Exception ex) {
                Console.WriteLine($"Cannot retrieve the quote: {ex.Message}");
                return (null, $"Cannot retrieve the quote: {ex.Message}");
            }
        }
    }
}
