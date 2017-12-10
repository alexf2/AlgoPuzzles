using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlgoPuzzles.Models;
using Algorithms;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace AlgoPuzzles.Controllers
{
    public class HomeController : Controller
    {
        readonly IAlgo[] _algos;

        public HomeController (IAlgo[] algos)
        {
            _algos = algos;
            Array.Sort(_algos, (a1, a2) => StringComparer.InvariantCultureIgnoreCase.Compare(a1.Name, a2.Name));
        }

        public IActionResult Index()
        {
            return View(new Registry(_algos));
        }

        public IActionResult Algo(int index)
        {
            return PartialView(new Registry(_algos, index));
        }

        [HttpPost]
        public IActionResult Execute([/*FromBody*/ FromForm] IFormCollection testCase, [FromRoute] int algoIndex)
        {
            return Json(testCase);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
