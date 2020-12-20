using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RTFM_SearchEngine.Api;
using RTFM_SearchEngine.Models;

namespace RTFM_SearchEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISearchApi searchApi;

        public HomeController(ILogger<HomeController> logger, ISearchApi searchApi)
        {
            _logger = logger;
            this.searchApi = searchApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SearchAsync(string searchText)
        {
            ViewData["lastSearch"] = searchText;
            
            return View(await searchApi.GetSearchResultAsync(searchText));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
