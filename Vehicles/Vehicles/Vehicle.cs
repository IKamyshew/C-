using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Vehicle
    {
        private String name { get; set; }
        protected double speedPerHour;
        private int capacity;
        private int safety;

        protected Vehicle(String name, double speedPerHour, int capacity, int safety)
        {
            this.name = name;
            this.speedPerHour = speedPerHour;
            this.capacity = capacity;
            this.safety = safety;
        }



        public abstract double TimeToDestinationPoint(double distance);
    }
}
