using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RTFM_SearchEngine.Models;

namespace RTFM_SearchEngine.Api
{

    public class GoogleSearchApi : ISearchApi
    {
        public async Task<List<SearchResult>> GetSearchResultAsync(string searchText)
        {
            string cleanedSearchText = searchText.Replace(" ", "+");
            cleanedSearchText = cleanedSearchText.Replace("'", "");
            cleanedSearchText = cleanedSearchText.Replace("\"", "");
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://google-search3.p.rapidapi.com/api/v1/search/q={cleanedSearchText}&num=50&lr=lang_it"),
                Headers =
                {
                    { "x-rapidapi-key", "1c2eb8ab42msh25eb17f236d532dp15b8dejsne52d0aab7277" },
                    { "x-rapidapi-host", "google-search3.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                dynamic jsonData = JsonConvert.DeserializeObject(body);
                var results = new List<SearchResult>();
                foreach (var item in jsonData.results)
                {
                    results.Add(new SearchResult{
                        Title = item.title,
                        Link = item.link,
                        Description = item.description
                    });
                }
                return results;
            }
        }
    }
}