using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class SPHModel
    {
        public string job_id { get; set; }

        public string[] weeks { get; set; }

        public int[] salaries { get; set; }
    }
}
