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
        private string Cost;

        protected Vehicle(string Name, double SpeedPerHour, int Capacity, int Safety, string Cost)
        {
            this.Name = Name;
            this.SpeedPerHour = SpeedPerHour;
            this.Capacity = Capacity;
            if (Safety <= 100 && Safety >= 0)
                this.Safety = Safety; 
            this.Cost = Cost;
        }

        public abstract double TimeToDestinationPoint(double distance);

        public override string ToString()
        {
            return "Vehicle characteristics: name: " + Name + "; Speed per hour: " + SpeedPerHour + "; Capacity: " + Capacity + "; Safety: " + Safety + " %; Cost: " + Cost + ";";
        }
    }
}
