using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    abstract class Vehicle
    {
        public string Name { get; protected set; }
        protected double SpeedPerHour;
        private int Capacity;
        private int Safety;

        protected Vehicle(string Name, double SpeedPerHour, int Capacity, int Safety)
        {
            this.Name = Name;
            this.SpeedPerHour = SpeedPerHour;
            this.Capacity = Capacity;
            this.Safety = Safety;
        }

        public abstract double TimeToDestinationPoint(double distance);
    }
}
