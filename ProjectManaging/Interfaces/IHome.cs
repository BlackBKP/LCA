using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Interfaces
{
    interface IHome
    {
        List<EmployeeModel> GetEmployees();
    }
}
