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

    [Route("DBHealth")]
    public class DBHealthController : Controller
    {
        Models.IMiningRepository _MiningRepository;

        public DBHealthController(Models.IMiningRepository _miningRepo)
        {
            _MiningRepository = _miningRepo;
        }

        [HttpGet(Name = "GetDBHeath")]
        public IActionResult Index()
        {
            try
            {
                var status = _MiningRepository.GetConnectionStatus();
                return Content($"Connection: {status} {DateTime.Now.ToLongTimeString()}");
            }
            catch (Exception)
            {
                return Content($"Connection failed! - {DateTime.Now.ToLongTimeString()}");
            }
            
        }
    }
}