using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Interfaces
{
    interface IHome
    {
        List<SpentPerWeekModel> GetSpentCostPerWeeks();
        List<SpentPerWeekModel> GetSpentPerWeeksByJob(string job_id);
        List<EmployeeModel> GetEmployees();
        List<HourModel> GetHours();
        List<JobModel> GetJobs();
        List<LaborCostModel> GetLaborCosts();
        List<OvertimeModel> GetOvertimes();
    }
}
