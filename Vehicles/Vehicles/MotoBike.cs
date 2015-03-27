using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class MotoBike : Bike
    {
        private static string Cost = "Moderate";
        
        public MotoBike(string Name, double SpeedPerHour, int Safety, int Wheels)
            : base(Name, SpeedPerHour, Safety, Wheels, Cost)
        {
            
        }

        public override double TimeToDestinationPoint(double Distance)
        {
            return Distance / SpeedPerHour;
        }
    }
}
