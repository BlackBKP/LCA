using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Models
{
    public class JobSummaryModel
    {
        public string job_id { get; set; }

        public int estimate_budget { get; set; }

        public double labor_cost { get; set; }

        public double labor_cost_ot { get; set; }

        public double accomodation_cost { get; set; }

        public double compensation_cost { get; set; }

        public double cost_to_date { get; set; }

        public double remainning_cost { get; set; }

        public double percent_cost_usage { get; set; }

        public double percent_work_completion { get; set; }

        public double normal_manhour { get; set; }

        public double x15_manhour { get; set; }

        public double x30_manhour { get; set; }

        public double total_manhour { get; set; }

        public int number_of_labor { get; set; }

        public double avg_labor_cost_per_hour { get; set; }

    }
}
