using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace liver.Controllers
{

    [Produces("application/json")]
    [Route("api/Miner")]
    public class MinerController : Controller
    {

        Models.IMiningRepository _MiningRepository;

        public MinerController(Models.IMiningRepository _miningRepo)
        {
            _MiningRepository = _miningRepo;
        }


        [HttpPost(Name = "Dig")]
        public Models.MineResult Dig(string clientID)
        {

            var _DifficultyLevel = _MiningRepository.GetDifficulty();

            if (_DifficultyLevel == 0)
                _DifficultyLevel = 1;
            
            Stopwatch watch = new Stopwatch();
            watch.Start();
            decimal x = 1;
            //for (int i = 0; i < _DifficultyLevel / 2 ; i++)
            //{
            //    x += 1;
            //    var cpuLoad = i * i;
            //    if (cpuLoad > 90)
            //    {
            //        cpuLoad = 90;
            //    }
            //    CPUKill(cpuLoad, 300);
            //}

            CPUSquareRootKill(Convert.ToInt32(_DifficultyLevel));

            var mined = x * (1 / _DifficultyLevel);
            watch.Stop();

            var newDig = new liver.Models.MiningDig() {
                 Client = clientID,
                 CoinsMined = mined,
                 MillisecondTaken = watch.ElapsedMilliseconds
            };

            _MiningRepository.Add(newDig);

            var stats = _MiningRepository.GetStats();

             return new Models.MineResult() {
                ResultID = -1,
                Difficulty = _DifficultyLevel,
                MinedCoins = mined,
                MilliSecondsTaken = watch.ElapsedMilliseconds,
                MinedCoinsTotal = stats.MinedCoinsTotal,
                NumberOfMiners = stats.NumberOfMiners,
                MinerHostName = Environment.MachineName
             };

        }
 


        public static void CPUKill(object cpuUsage, decimal milliseconds)
        {
            Parallel.For(0, 1, new Action<int>((int i) =>
            {
                Stopwatch watch = new Stopwatch();
                Stopwatch overallWatch = new Stopwatch();
                overallWatch.Start();
                watch.Start();
                while (overallWatch.ElapsedMilliseconds < milliseconds)
                {
                    if (watch.ElapsedMilliseconds > Convert.ToInt32(cpuUsage))
                    {
                        Thread.Sleep(100 - Convert.ToInt32(cpuUsage));
                        watch.Reset();
                        watch.Start();
                    }
                }
            }));

        }

        public static void CPUSquareRootKill(int difficulty)
        {

            System.Threading.Tasks.Parallel.For(1, difficulty * difficulty, y => {
                var x = 0.0001;
                for (int i = 0; i < difficulty * 1000000; i++)
                {
                    x = Math.Sqrt(x);
                }
            });

        }

    }
}