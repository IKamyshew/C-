using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Automobile : Vehicle
    {
        private static int Safety = 50;
        private static string Cost = "Moderate";

        public Automobile(string Name, double SpeedPerHour, int Capacity)
            : base(Name, SpeedPerHour, Capacity, Safety, Cost)
        {
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
