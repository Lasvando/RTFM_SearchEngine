using System.Collections.Generic;
using System.Threading.Tasks;
using RTFM_SearchEngine.Models;

namespace RTFM_SearchEngine.Api
{
    public interface ISearchApi
    {
        Task<List<SearchResult>> GetSearchResultAsync(string searchText);
    }
}