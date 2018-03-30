using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace liver.Controllers
{
    [Route("Health")]
    public class HealthController : Controller
    {


        [HttpGet(Name = "GetHeath")]
        public IActionResult Index()
        {
            return Content($"Healthy! Running on {Environment.MachineName} - time: {DateTime.Now.ToLongTimeString()}");
        }
    }
}