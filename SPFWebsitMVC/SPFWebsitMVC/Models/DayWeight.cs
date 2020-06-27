using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SPFWebsitMVC.Models
{
    public class DayWeight
    {
        [Key]
        public int DayWeightId { get; set; }

        [Display(Name = "Day Number")]
        public int Day { get; set; }

        [Display(Name = "Weight(lbs)")]
        public double Weight { get; set; }

        public int ClientId { get; set; }
    }
}
