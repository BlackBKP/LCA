using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class JobModel
    {
        public string job_id { get; set; }

        public string job_name { get; set; }

        public int estimated_budget { get; set; }

        public int cost_to_date { get; set; }

        public int remaining_cost { get; set; }

        public int cost_usage { get; set; }

        public int work_completion { get; set; }

        public int total_normal_man_hour { get; set; }

        public int no_of_labor { get; set; }

        public int avg_cost_per_hour { get; set; }

        public int job_year { get; set; }

        public DateTime lastest_update { get; set; }
    }
}
