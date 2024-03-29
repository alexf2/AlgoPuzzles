﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AlgoPuzzles.Models;
using Algorithms;
using Microsoft.AspNetCore.Http;
using AlgoPuzzles.Helpers;


namespace AlgoPuzzles.Controllers
{    
    public class HomeController : Controller
    {
        readonly IAlgo[] _algos;
        readonly IAlgoManager _mgr;

        public HomeController (IAlgo[] algos, IAlgoManager mgr)
        {
            _algos = algos;
            Array.Sort(_algos, (a1, a2) => StringComparer.InvariantCultureIgnoreCase.Compare(a1.Name, a2.Name));
            _mgr = mgr;
        }
        
        public IActionResult Index()
        {
            return View(new Registry(_algos));
        }

        [ActionName("algorithm")]
        public IActionResult Algo([FromRoute(Name = "id")] int algoIndex)
        {
            return PartialView("Algo", new Registry(_algos, algoIndex));
        }

        [Produces(typeof(string))]
        //[HttpGet("/home/code/{algoIndex}")]
        [ActionName("code")]
        public async Task<IActionResult> SourceCode ([FromRoute(Name = "id")] int algoIndex)
        {         
            return Content(await _mgr.LoadCode(_algos[algoIndex]));
        }

        [HttpPost]
        public async Task<IActionResult> Execute([/*FromBody*/ FromForm] IFormCollection testCase, [FromRoute(Name = "id")] int algoIndex)
        {
            var algo = _algos[ algoIndex ];
            
            var sample = Activator.CreateInstance(algo.ParamsType);
            bool status = await TryUpdateModelAsync(sample, algo.ParamsType, string.Empty);
            if (!status)
                throw new Exception($"Unable to get model {algo.ParamsType.Name} from the form for {algo.Name}.\r\n{ModelErrors}");
            
            dynamic res;
            using (new Timing(this.ViewBag))
                res = await algo.Execute(sample);


            return PartialView(res);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        string ModelErrors => string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
    }
}
