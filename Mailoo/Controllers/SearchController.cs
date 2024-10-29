using Mailo.Data;
using Mailo.IRepo;
using Mailo.Models;
using Mailo.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mailo.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchRepo _search;
        public SearchController(ISearchRepo search)
        {
            _search=search;
        }
        public IActionResult Index(string text)
        {
            return View(_search.Search(text));
        }
    }
}
