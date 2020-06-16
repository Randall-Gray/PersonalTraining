using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Models
{
    public class FAQ
    {
        [Key]
        public int FAQId { get; set; }

        // A general FAQ is posted on the outer pages before client login.
        public bool General { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DatePosted { get; set; }

        [Display(Name = "Asked By")]
        public string ClientName { get; set; }

        [Display(Name = "Answered By")]
        public string TrainerName { get; set; }
    }
}
