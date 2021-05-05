using ProjectManaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManaging.Services
{
    public class ConnectDB : IConnectDB
    {
        public void Connect()
        {
            Console.WriteLine("Connect DB");
        }
    }
}
