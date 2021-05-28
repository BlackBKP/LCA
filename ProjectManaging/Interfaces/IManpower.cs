using ProjectManaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Interfaces
{
    interface IManpower
    {
        List<List<MPHModel>> GetMPHModels();
        List<List<MPHModel>> GetMPHModels(string job_id);
    }
}
