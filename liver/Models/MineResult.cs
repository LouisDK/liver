using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace liver.Models
{
    public class MineResult
    {
        [Key]
        public long ResultID { get; set; }
            
        public decimal MinedCoins { get; set; }

        public decimal Difficulty { get; set; }
        public long MilliSecondsTaken { get; internal set; }
        public decimal MinedCoinsTotal { get; internal set; }
        public int NumberOfMiners { get; internal set; }
        public string MinerHostName { get; internal set; }
    }
}
