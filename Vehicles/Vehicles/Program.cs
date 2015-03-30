using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicles
{
    class Program
    {
        static void Main(string[] args)
        {
            Bike yamahaR6 = new MotoBike("yamaha r6", 165);
            Console.WriteLine("From Kiev to Lviv " + yamahaR6.Name + " will arrive in: " + yamahaR6.TimeToDestinationPoint(541.1) + " hours");
            Console.WriteLine(yamahaR6);
            Console.WriteLine();

            Bike winnerAvatar24 = new Bicycle("Winner Avatar 24", 25);
            Console.WriteLine("From Kiev to Lviv " + winnerAvatar24.Name + " will arrive in: " + winnerAvatar24.TimeToDestinationPoint(541.1) + " hours");
            Console.WriteLine(winnerAvatar24);
            Console.WriteLine();

            Plane airbusA380 = new Plane("airbus a380", 900, 480);
            Console.WriteLine("From Kiev to Lviv " + airbusA380.Name + " will arrive in: " + airbusA380.TimeToDestinationPoint(541.1) + " hours");
            Console.WriteLine(airbusA380);
            Console.WriteLine();

            Automobile dodge_Charger_5_7_AWD = new Automobile("Dodge Charger 5.7 AWD", 150, 2);
            Console.WriteLine("From Kiev to Lviv " + dodge_Charger_5_7_AWD.Name + " will arrive in: " + dodge_Charger_5_7_AWD.TimeToDestinationPoint(541.1) + " hours");
            Console.WriteLine(dodge_Charger_5_7_AWD);
            Console.WriteLine();

            Train hyundai = new Train("Hyundai", 100, 100);
            Console.WriteLine("From Kiev to Lviv " + hyundai.Name + " will arrive in: " + hyundai.TimeToDestinationPoint(541.1) + " hours");
            Console.WriteLine(hyundai);
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
