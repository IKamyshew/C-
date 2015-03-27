using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Program
    {
        static void Main(string[] args)
        {
            MotoBike yamahaR6 = new MotoBike("yamaha r6", 120, 30, 2);
            Console.WriteLine("From Kiev to Lviv " + yamahaR6.Name + " will arrive in: " + yamahaR6.TimeToDestinationPoint(541.1) + " hours");
            
            Console.ReadKey();
        }
    }
}
