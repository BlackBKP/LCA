using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class JobModel
    {
        public string job_id { get; set; }

        public string[] fortnight { get; set; }

        public double[] progress { get; set; }

        public double[] spent { get; set; }

        public int budget { get; set; }

        public string pm { get; set; }
    }
}
