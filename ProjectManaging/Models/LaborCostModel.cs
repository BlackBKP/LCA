using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class LaborCostModel
    {
        public string job_id { get; set; }

        public int week { get; set; }

        public int month { get; set; }

        public string week_time { get; set; }

        public int labor_cost { get; set; }

        public int ot_labor_cost { get; set; }

        public int accommodation_cost { get; set; }

        public int compensation_cost { get; set; }

        public int no_of_labor_week { get; set; }
    }
}
