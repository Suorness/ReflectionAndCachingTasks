using CachTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePLCachTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = "WTF";
            var cachHelper = new CachHelper();
            cachHelper.SetInCach("TeSt", key);
            Console.WriteLine(cachHelper.GetFromCach(key));
            Console.ReadLine();
        }
    }
}
