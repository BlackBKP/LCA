using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class MPHModel
    {
        public string job_id { get; set; }

        public string[] week { get; set; }

        public int[] normal { get; set; }

        public int[] overtime { get; set; }
    }
}