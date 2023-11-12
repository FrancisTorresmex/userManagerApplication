using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using userManagerAplication.Models.Data;
using userManagerApplication.Indentity;
using userManagerApplication.Models;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Controllers
{
    //[Authorize(Policy = IdentityData.UserPolicyName)]
    [Authorize(Policy = "UserPolicy")]

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<UsersRole> _repository;
        

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IGenericRepository<UsersRole> repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            string token = HttpContext.Request.Cookies["Token"];

            var tokenHelper = new TokenHelper(_configuration);
            if (tokenHelper.ValidateTokenAccess(token))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
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