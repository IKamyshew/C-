using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Bicycle : Bike
    {
        private static string Cost = "Free";
        private static int Safety = 70;

        public Bicycle(string Name, double SpeedPerHour)
            : base(Name, SpeedPerHour, Safety, Cost)
        {
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
