using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class QuickSort
    {
        // Data is saved statically to allow search to function.
        private static List<int> data;
        private static long steps;

        // Quick sort constructor.
        public QuickSort(List<int> data_, int sortDirection)
        {
            data = data_;
            steps = 0;
            Sort(0, data.Count - 1, sortDirection);
        }

        public List<int> getData()
        {
            return data;
        }

        public long getSteps() {
            return steps;
        }

        /// <summary>
        /// Header function for the quicksort algorithm.
        /// </summary>
        /// <param name="lowerBoundary">Lower boundary of the partition.</param>
        /// <param name="upperBoundary">Upper boundary of the partition.</param>
        /// <param name="direction">Direction to sort in. 1: Ascending, -1: Descending.</param>
        private static void Sort(int lowerBoundary, int upperBoundary, int direction)
        {
            // While the boundaries haven't crossed:
            if (lowerBoundary < upperBoundary)
            {
                // Create a partition to determine the split index.
                int partitionIndex = Partition(lowerBoundary, upperBoundary, direction);
                // Create partitions either side of the split index.
                Sort(lowerBoundary, partitionIndex - 1, direction);
                steps++;
                Sort(partitionIndex + 1, upperBoundary, direction);
                steps++;
            }

        }

        private static int Partition(int lowerBoundary, int upperBoundary, int direction)
        {
            // Set the pivot value as the last value in the partition.
            int pivotVal = data[upperBoundary];
            int lowerIndex = (lowerBoundary - 1);
            int temp;

            // Iterate through the partition.
            for (int j = lowerBoundary; j < upperBoundary; j++)
            {
                // Switch values that are lower than the pivot value.
                if ((data[j] * direction) <= (pivotVal * direction))
                {
                    lowerIndex++;

                    temp = data[lowerIndex];
                    data[lowerIndex] = data[j];
                    steps++;
                    data[j] = temp;
                    steps++;
                }
            }

            temp = data[lowerIndex + 1];
            data[lowerIndex + 1] = data[upperBoundary];
            steps++;
            data[upperBoundary] = temp;
            steps++;
            return lowerIndex + 1;
        }

    }
}
