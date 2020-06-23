using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SPFWebsitMVC.Models
{
    public class BroadcastMessage
    {
        [Key]
        public int BroadcastMessageId { get; set; }

        public string Message { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePosted { get; set; }

        public int NumDays { get; set; }

        [Display(Name = "Posted By")]
        public string PosterName { get; set; }
    }
}
