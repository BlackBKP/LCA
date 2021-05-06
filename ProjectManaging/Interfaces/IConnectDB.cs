using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Interfaces
{
    public interface IConnectDB
    {
        SqlConnection Connect();
    }
}
