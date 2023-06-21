using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StomatoloskaOrdinacija.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace StomatoloskaOrdinacija.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public zubarContext _context { get; set; }        
        public HomeController(ILogger<HomeController> logger, zubarContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var blogs = _context.blogs;
            ViewBag.Blogs = blogs;
            return View();
        }
        public IActionResult ShowBlog(int blogid)
        {
            var blog = _context.blogs.Where(b => b.BlogID == blogid).FirstOrDefault();
            ViewBag.Blog = blog;
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Secret()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["returnedUrl"] = ReturnUrl;
            return View();
        }

       


        [HttpGet("/Home/Logout")]
        public IActionResult Odjavi()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Home");
        }


        [HttpPost("/Home/Login")]
        public IActionResult Proveri(string username,string password,string ReturnUrl)
        {
            if(username == "lazar" && password == "123")
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(ReturnUrl);
            }
            return BadRequest();
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