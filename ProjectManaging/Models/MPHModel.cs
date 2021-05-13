using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class MPHModel
    {
        public string job_id { get; set; }

        public int week { get; set; }

        public int month { get; set; }

        public int normal { get; set; }

        public int ot_1_5 { get; set; }

        public int ot_3 { get; set; }

        public int acc_hour { get; set; }

    }
}