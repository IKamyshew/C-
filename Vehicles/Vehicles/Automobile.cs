using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Automobile : Vehicle
    {
        private int wheels;

        protected Automobile(String name, double speedPerHour, int safety, int capacity, int wheels)
            : base(name, speedPerHour, capacity, safety)
        {
            this.wheels = wheels;
        }
    }
}
