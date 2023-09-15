using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    internal class Clients
    {
        public static ConcurrentDictionary<StreamWriter, bool> allClients = new();
    }
}
