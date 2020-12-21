using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Newtonsoft.Json;
using RTFM_SearchEngine.Models;

namespace RTFM_SearchEngine.Api
{

    public class GoogleSearchApi : ISearchApi
    {
        public async Task<List<SearchResult>> GetSearchResultAsync(string searchText)
        {
            string cleanedSearchText = searchText.Replace(" ", "%20");
            cleanedSearchText = cleanedSearchText.Replace("'", "");
            cleanedSearchText = cleanedSearchText.Replace("\"", "");

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://google-search5.p.rapidapi.com/google-serps/?q={cleanedSearchText}&gl=it&hl=it-IT"),
                Headers =
                {
                    { "x-rapidapi-key", "1c2eb8ab42msh25eb17f236d532dp15b8dejsne52d0aab7277" },
                    { "x-rapidapi-host", "google-search5.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                dynamic jsonData = JsonConvert.DeserializeObject(body);
                var results = new List<SearchResult>();
                foreach (var item in jsonData.data.results.organic)
                {
                    results.Add(new SearchResult
                    {
                        Title = item.title,
                        Link = item.url,
                        Description = item.snippet
                    });
                }
                return results;
            }
        }
    }
}