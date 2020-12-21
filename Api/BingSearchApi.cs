using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RTFM_SearchEngine.Models;


namespace RTFM_SearchEngine.Api
{
    public class BingSearchApi : ISearchApi
    {
        public async Task<List<SearchResult>> GetSearchResultAsync(string searchText)
        {
            //KEY IN Api/ApiKeyHidden.cs
            //Keys are note available, pls go to Rapidapi and subscribe to obtain free Api's keys

            string key = ApiKeyHidden.BING_API_KEY;

            string cleanedSearchText = searchText.Replace(" ", "%20");
            cleanedSearchText = cleanedSearchText.Replace("'", "");
            cleanedSearchText = cleanedSearchText.Replace("\"", "");

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://bing-web-search1.p.rapidapi.com/search?q={cleanedSearchText}&mkt=it-it"),
                Headers =
                {
                    { "x-bingapis-sdk", "true" },
                    { "x-rapidapi-key",  key},
                    { "x-rapidapi-host", "bing-web-search1.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                dynamic jsonData = JsonConvert.DeserializeObject(body);
                var results = new List<SearchResult>();
                foreach (var item in jsonData.webPages.value)
                {
                    results.Add(new SearchResult
                    {
                        Title = item.name,
                        Link = item.url,
                        Description = item.snippet
                    });
                }
                return results;
            }
        }
    }
}