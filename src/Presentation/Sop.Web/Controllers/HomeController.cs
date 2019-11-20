using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sop.Data.Test.Models;
using Sop.Domain.Entities;
using Sop.Domain.Services;
using Sop.Web.Models;

namespace Sop.Web.Controllers
{
    public class HomeController : Controller
    {
        public readonly ITestService TextService;
        public HomeController(TestService textService)
        {
            TextService = textService;
        }

        public IActionResult Index()
        {

            var info = new Tests()
            {
                IsDel = true,
                Body = "TEST",
                DateCreated = DateTime.Now,
                DecimalValue = 123.34M,
                FloatValue = 123322.32f,
                LongValue = 1467896543L,
                Status = false,
                Type = TestType.Again
            };

            TextService.Create(info);


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
    }
}
