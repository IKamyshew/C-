using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class MotoBike : Bike
    {
        private static int Safety = 30;
        private static string Cost = "Moderate";

        public MotoBike(string Name, double SpeedPerHour)
            : base(Name, SpeedPerHour, Safety, Cost)
        {
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
