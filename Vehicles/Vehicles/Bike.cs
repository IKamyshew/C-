using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Bike : Vehicle
    {
        
        private int Wheels;
        private static int Capacity = 1;

        protected Bike(string Name, double SpeedPerHour, int Safety, int Wheels)
            : base (Name, SpeedPerHour, Capacity, Safety)
        {
            this.Wheels = Wheels;
        }
    }
}
