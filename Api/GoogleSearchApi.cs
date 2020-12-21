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
            //KEY IN Api/ApiKeyHidden.cs
            //Keys are note available, pls go to Rapidapi and subscribe to obtain free Api's keys

            string key = ApiKeyHidden.GOOGLE_API_KEY;

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
                    { "x-rapidapi-key", key },
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