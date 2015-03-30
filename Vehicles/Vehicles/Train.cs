using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Train : Vehicle
    {
        private static int Safety = 90;
        private static string Cost = "Cheap";

        public Train(string Name, double SpeedPerHour, int Capacity)
            : base(Name, SpeedPerHour, Capacity, Safety, Cost)
        {
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
