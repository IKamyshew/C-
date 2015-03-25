using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortings
{
    class SortTypes
    {
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

        public static int[] selectionSort(int[] array)
        {
            int[] innerArray = new int[array.Length];
            Array.Copy(array, innerArray, array.Length);

            for (int i = 0; i < innerArray.Length; i++)
            {
                int temp;
                for (int j = i + 1; j < innerArray.Length; j++)
                {
                    if (innerArray[j] < innerArray[i])
                    {
                        temp = innerArray[i];
                        innerArray[i] = innerArray[j];
                        innerArray[j] = temp;
                    }
                }
            }

            return innerArray;
        }

        public static int[] shellSort(int[] array)
        {
            int[] innerArray = new int[array.Length];
            Array.Copy(array, innerArray, array.Length);

            int j;
            int step = innerArray.Length / 2;
            while (step > 0)
            {
                for (int i = 0; i < (innerArray.Length - step); i++)
                {
                    j = i;
                    while ((j >= 0) && (innerArray[j] > innerArray[j + step]))
                    {
                        int tmp = innerArray[j];
                        innerArray[j] = innerArray[j + step];
                        innerArray[j + step] = tmp;
                        j -= step;
                    }
                }
                step = step / 2;
            }
            return innerArray;
        }
    }
}
