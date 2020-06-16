using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Models
{
    public class ExerciseClass
    {
        [Key]
        public int ExerciseClassId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        [Display(Name = "Trainer")]
        public string TrainerName { get; set; }

        public DateTime Schedule { get; set; }

        [Display(Name = "Maximum Size")]
        public int MaxSize { get; set; }
    }
}
