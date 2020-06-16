using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Models
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public string Link { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePosted { get; set; }

        [Display(Name = "Current User Favorite")]
        public int CurrentUse { get; set; }

        [Display(Name = "Total User Favorite")]
        public int TotalUse { get; set; }

        [Display(Name = "Post?")]
        public bool Post { get; set; }
    }
}
