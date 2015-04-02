using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CollectionsTrainings
{
    class Program
    {
        static void Main(string[] args)
        { 
            List<int> list = new List<int>(5);
            list.Add(1665561);
            list.Add(2);
            list.Add(1);

            readArray(list);

            Console.WriteLine(list.Contains(2));
            Console.WriteLine(list.ElementAt(1));
            Console.WriteLine(list[0].Equals(list[2]));

            list[1] = 5;

            readArray(list);

            Console.ReadKey();
        }

        public static void readArray (List<int> array) {
            foreach(var variable in array) {
                Console.WriteLine(variable);
            }

            Console.ReadKey();
        }
    }
}
