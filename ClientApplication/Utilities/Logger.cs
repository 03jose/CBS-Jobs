using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication.Utilities
{
    public class Logger
    {
        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }
    }
}
