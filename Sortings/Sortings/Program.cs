using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Console.WriteLine(string.Join(",", array));

            Console.WriteLine("Press any key to perform Insertion sort.");
            Console.ReadKey();
            Console.WriteLine(string.Join(",", Program.insertionSort(array)));

            Console.WriteLine("Press any key to perform Bubble sort.");
            Console.ReadKey();
            Console.WriteLine(string.Join(",", Program.insertionSort(array)));

            Console.WriteLine("Press any key to perform Shaker sort.");
            Console.ReadKey();
            Console.WriteLine(string.Join(",", Program.shakerSort(array)));

            Console.ReadKey();
        }

        public static int[] insertionSort(int[] array)
        {
            int[] innerArray = new int[array.Length];
            Array.Copy(array, innerArray, array.Length);

            for (int i = 1; i < innerArray.Length; i++)
            {
                int temp = innerArray[i];
                for (int j = i - 1; j >= 0 && innerArray[j] > temp; j--)
                {
                    innerArray[j + 1] = innerArray[j];
                    innerArray[j] = temp;
                }
            }

            return innerArray;
        }

        public static int[] bubbleSort(int[] array)
        {
            int[] innerArray = new int[array.Length];
            Array.Copy(array, innerArray, array.Length);

            for (int i = 1; i < innerArray.Length; i++)
            {
                int temp = -1;
                for (int j = 1; j < innerArray.Length; j++)
                {
                    if (innerArray[j - 1] > innerArray[j])
                    {
                        temp = innerArray[j];
                        innerArray[j] = innerArray[j - 1];
                        innerArray[j - 1] = temp;
                    }
                }
            }

            return innerArray;
        }

        public static int[] shakerSort(int[] array)
        {
            int[] innerArray = new int[array.Length];
            Array.Copy(array, innerArray, array.Length);

            for (int i = 1; i < innerArray.Length; i++)
            {
                int temp = -1;
                for (int j = 1; j < innerArray.Length; j++)
                {
                    if (innerArray[j - 1] > innerArray[j])
                    {
                        temp = innerArray[j];
                        innerArray[j] = innerArray[j - 1];
                        innerArray[j - 1] = temp;
                    }
                }
                for (int j = innerArray.Length - 1; j > 0; j--)
                {
                    if (innerArray[j - 1] > innerArray[j])
                    {
                        temp = innerArray[j];
                        innerArray[j] = innerArray[j - 1];
                        innerArray[j - 1] = temp;
                    }
                }
            }

            return innerArray;
        }
    }
}
