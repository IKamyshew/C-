using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

/* 
 * insertion sort O(n^2)
 * bubble sort    O(n^2)
 * shaker sort
 * selection sort O(n^2)
 */
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            

            int[] array = new int[15];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next(100);
            }

            Console.WriteLine(string.Join(",", array) + " | Time(ms):");

            Console.WriteLine("Performing Insertion sort.");
            sortArray(string.Join(",", Sortings.SortTypes.insertionSort(array)));


            Console.WriteLine("Performing Bubble sort.");
            sortArray(string.Join(",", Sortings.SortTypes.bubbleSort(array)));

            Console.WriteLine("Performing Shaker sort.");
            sortArray(string.Join(",", Sortings.SortTypes.shakerSort(array)));

            Console.WriteLine("Performing Selection sort.");
            sortArray(string.Join(",", Sortings.SortTypes.selectionSort(array)));

            Console.WriteLine("Performing Shell sort.");
            sortArray(string.Join(",", Sortings.SortTypes.shellSort(array)));

            Console.ReadKey();
        }

        public static void sortArray(string sortedArray)
        {
            Stopwatch time = new Stopwatch();
            time.Start();
            Console.Write(sortedArray);
            time.Stop();
            Console.WriteLine(" | " + time.Elapsed.TotalMilliseconds);
            time.Reset();
        }
    }
}
