using Book_Store_App.Models;
using Book_Store_App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book_Store_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailServices _emailServices;

        public HomeController(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
