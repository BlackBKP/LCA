using ProjectManaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Services
{
    public class HomeService : IHome
    {
        IConnectDB connect;
        public HomeService()
        {
            this.connect= this.connect = new ConnectDB();
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string Show()
        {
            string s = connect.Connect();
            return s;
        }
    }
}
