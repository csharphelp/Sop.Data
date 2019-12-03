using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sop.Domain.Domain.Services;
using Sop.Web.Models;

namespace Sop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;

        private readonly IValuesService _valuesService;

        public HomeController(IValuesService valuesService, IPostService postService)
        {
            _valuesService = valuesService;
            _postService = postService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var post = _postService.Find(1);


            return _valuesService.FindAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _valuesService.Find(id);
        }

        public IActionResult Index()
        {
            var post = _postService.Find(1);


            //var info = new Tests()
            //{
            //    IsDel = true,
            //    Body = "TEST",
            //    DateCreated = DateTime.Now,
            //    DecimalValue = 123.34M,
            //    FloatValue = 123322.32f,
            //    LongValue = 1467896543L,
            //    Status = false,
            //    Type = TestType.Again
            //};

            //TextService.Create(info);


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}