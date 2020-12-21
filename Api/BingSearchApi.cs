using System;
using System.Collections.Generic;
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
                    { "x-rapidapi-key", "1c2eb8ab42msh25eb17f236d532dp15b8dejsne52d0aab7277" },
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