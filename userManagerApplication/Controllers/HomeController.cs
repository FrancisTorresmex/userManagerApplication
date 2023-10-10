using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<UsersRole> _repository;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<UsersRole> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<UsersRole> users = _repository.GetAll();
            return View("Index", users);
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