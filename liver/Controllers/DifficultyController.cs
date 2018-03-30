using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace liver.Controllers
{
    [Produces("application/json")]
    [Route("api/Difficulty")]
    public class DifficultyController : Controller
    {

        Models.IMiningRepository _MiningRepository;

        public DifficultyController(Models.IMiningRepository _miningRepo)
        {
            _MiningRepository = _miningRepo;
        }


        [HttpPost(Name = "SetDifficulty")]
        public decimal SetDifficulty(int diff)
        {

            _MiningRepository.SetDifficulty(diff);

            var setdiff = _MiningRepository.GetDifficulty();
            return setdiff;

        }

        [HttpGet(Name = "GetDifficulty")]
        public decimal GetDifficulty()
        {

            var setdiff = _MiningRepository.GetDifficulty();
            return setdiff;

        }

    }
}