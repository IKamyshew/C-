using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Bike : Vehicle
    {
        private static int Capacity = 1;

        protected Bike(string Name, double SpeedPerHour, int Safety, string Cost)
            : base (Name, SpeedPerHour, Capacity, Safety, Cost)
        {
        }
    }
}
