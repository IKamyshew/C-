using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Plane : Vehicle
    {
        private static string Cost = "Expensive";
        private static int Safety = 85;
        public Plane(string Name, double SpeedPerHour, int Capacity)
            : base(Name, SpeedPerHour, Capacity, Safety, Cost)
        {
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
