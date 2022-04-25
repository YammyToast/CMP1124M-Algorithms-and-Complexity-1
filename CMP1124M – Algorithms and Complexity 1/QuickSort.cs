using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class QuickSort
    {
        private static List<int> data;
        private static long steps;

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

        private static void Sort(int lowerBoundary, int upperBoundary, int direction)
        {
            
            if (lowerBoundary < upperBoundary)
            {
                int partitionIndex = Partition(lowerBoundary, upperBoundary, direction);
                Sort(lowerBoundary, partitionIndex - 1, direction);
                steps++;
                Sort(partitionIndex + 1, upperBoundary, direction);
                steps++;
            }

        }

        private static int Partition(int lowerBoundary, int upperBoundary, int direction)
        {
            int pivotVal = data[upperBoundary];
            int lowerIndex = (lowerBoundary - 1);

            for (int j = lowerBoundary; j < upperBoundary; j++)
            {
                if ((data[j] * direction) <= (pivotVal * direction))
                {
                    lowerIndex++;

                    int temp = data[lowerIndex];
                    data[lowerIndex] = data[j];
                    steps++;
                    data[j] = temp;
                    steps++;
                }
            }

            int temp1 = data[lowerIndex + 1];
            data[lowerIndex + 1] = data[upperBoundary];
            steps++;
            data[upperBoundary] = temp1;
            steps++;
            return lowerIndex + 1;
        }


        //    public static void Sort(List<int> list, int lowerBoundary, int upperBoundary, int direction)
        //    {
        //        if (lowerBoundary < upperBoundary) { 
        //            int partition = QSPartition(list, lowerBoundary, upperBoundary, direction);
        //            Sort(list, lowerBoundary, partition - 1, direction);
        //            Sort(list, partition + 1, upperBoundary, direction);


        //        }

        //    }

        //    private static int QSPartition(List<int> partition, int lowerBoundary, int upperBoundary, int direction)
        //    {
        //        Console.WriteLine($"L: {lowerBoundary}, U: {upperBoundary}");
        //        //foreach (int item in partition) {
        //        //    Console.Write($" {item}");
        //        //}
        //        Console.WriteLine();
        //        int temp;
        //        int pivot = partition.ElementAt(lowerBoundary);
        //        int pivotIndex = lowerBoundary;
        //        for (int scanIndex = lowerBoundary + 1; scanIndex < upperBoundary; scanIndex++)
        //        {

        //            if ((partition.ElementAt(scanIndex) * direction) < (pivot * direction))
        //            {
        //                temp = partition.ElementAt(lowerBoundary);
        //                partition[lowerBoundary] = partition[scanIndex];
        //                partition[scanIndex] = temp;

        //                lowerBoundary++;
        //            }
        //        }
        //        temp = partition.ElementAt(lowerBoundary);
        //        partition[lowerBoundary] = partition.ElementAt(pivotIndex);
        //        partition[pivotIndex] = temp;

        //        return lowerBoundary;

        //    }
        //}
    }
}
