using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Display(Name = "User Name")]
        public string LoginName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Balance Owed")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double BalanceOwed { get; set; }

        public string Goal { get; set; }

        [Display(Name = "Favoriate Video 1")]
        public int FavoriteVideo1 { get; set; }

        [Display(Name = "Favoriate Video 2")]
        public int FavoriteVideo2 { get; set; }

        [Display(Name = "Favoriate Video 3")]
        public int FavoriteVideo3 { get; set; }
    }
}
