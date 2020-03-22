using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationClient.Models;

namespace WebApplicationClient.Controllers
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
            return View();
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

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token"); // you can get refresh_token just like that.

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }

        public async Task<IActionResult> GenerateBlog()
        {
            var payload = new Blog()
            {
                Name = $"Hakan Altındiş-{DateTime.Now.ToString("yyyyMMddHHmmssffff")}",
                Url = "http://www.google.com",
                Posts = new System.Collections.Generic.List<Post>
                {
                    new Post { Content = "Historical Infomation ", Title = "History"},
                    new Post { Content = "Technology Infomation ", Title = "Technology"}
                }
            };

            var accessToken = await HttpContext.GetTokenAsync("access_token"); // you can get refresh_token just like that.

            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var result = await client.PostAsync("http://localhost:5001/api/blog", content);

            ViewBag.Json = result.Content.ReadAsStringAsync().Result;
            return View("Json");
        }

        public async Task<IActionResult> GetBlogs()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token"); // you can get refresh_token just like that.

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/api/blog");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }

        public async Task<IActionResult> GetPosts()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token"); // you can get refresh_token just like that.

            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:5001/api/blog/posts");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("Json");
        }
    }
}
