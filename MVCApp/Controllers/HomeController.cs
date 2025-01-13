using Microsoft.AspNetCore.Mvc;
using MVCApp.Mapping;
using MVCApp.Models;
using System;
using System.Diagnostics;
using System.Text;
using TinyCsvParser;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            ContactMapping csvMapper = new ContactMapping();
            CsvParser<Contact> csvParser = new CsvParser<Contact>(csvParserOptions, csvMapper);
            var result = csvParser
                .ReadFromFile(@"data.csv", Encoding.ASCII)
                .ToList();

            var firstNames = result.GroupBy(x => x.Result.FirstName).Select(x => new Summary() { Name = x.Key , Count = x.Count() }).ToList();
            var lastNames = result.GroupBy(x => x.Result.LastName).Select(x => new Summary() { Name = x.Key , Count = x.Count() }).ToList();

            var addresses = result
                .Select(x => new { 
                    full = x.Result.Address, 
                    withoutnumber = string.Join("", x.Result.Address.Trim().Split().Skip(1)) 
                    })
                .OrderBy(x => x.withoutnumber)
                .Select(x => x.full).ToList();
            var viewModel = new HomeViewModel
            {
                Summaries = firstNames.Union(lastNames)
                                    .OrderByDescending(x => x.Count)
                                    .ThenBy(x => x.Name)
                                    .ToList(),
                Adresses = addresses
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
