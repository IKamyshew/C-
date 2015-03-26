using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Bike : Vehicle
    {
        private int wheels;
        private static int capacity = 1;

        protected Bike(String name, double speedPerHour, int safety, int wheels)
            : base (name, speedPerHour, capacity, safety)
        {
            this.wheels = wheels;
        }
    }
}
