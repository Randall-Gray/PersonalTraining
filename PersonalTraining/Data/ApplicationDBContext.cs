using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Data
{
    public class ApplicationDBContext : DbContext
    {
        // Member variables

        // Constructor
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        // Member methods
    }
}
