using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager1 = new Manager("Alex", "Smith");
            manager1.AddStudent("Vasya", "Popov");
            

            Console.ReadKey();
        }
    }
}
