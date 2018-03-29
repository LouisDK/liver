using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace liver.Controllers
{
    public class HealthController : Controller
    {

        [Route("Health")]
        public IActionResult Index()
        {
            return Content($"Healthy! Running on {Environment.MachineName} - time: {DateTime.Now.ToLongTimeString()}");
        }
    }
}