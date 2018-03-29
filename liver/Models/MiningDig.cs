using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace liver.Models
{
    public class MiningDig
    {
        [Key]
        [Display(Name = "Mining Dig Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client GUID")]
        [StringLength(500, ErrorMessage = "Name should be 1 to 500 char in lenght")]
        public string Client { get; set; }

        [Display(Name = "CoinsMined")]
        public decimal CoinsMined { get; set; }

        [Display(Name = "Millisecond taken")]
        public decimal MillisecondTaken { get; set; }


    }
}
