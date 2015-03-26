using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class MotoBike : Bike
    {
        public MotoBike(String name, double speedPerHour, int safety, int wheels)
            : base(name, speedPerHour, safety, wheels)
        {
            
        }

        public override double TimeToDestinationPoint(double distance)
        {
            return distance / speedPerHour;
        }
    }
}
