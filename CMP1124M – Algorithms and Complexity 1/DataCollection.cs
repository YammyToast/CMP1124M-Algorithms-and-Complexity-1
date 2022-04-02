using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class DataCollection
    {

        private List<int> data;
        private int dataCount;
        private int dataInterval;
        public readonly string fileName;

        public DataCollection(List<int> _data, string _fileName) { 
            data = _data;
            dataCount = _data.Count;
            fileName = _fileName;
            switch (dataCount)
            {
                case (256):
                    dataInterval = 10;
                    break;
                case (2048):
                    dataInterval = 50;
                    break;
            }
        }
        /// <summary>
        /// Gets the amount of data values in the data collection.
        /// </summary>
        /// <returns></returns>
        public int getCount() {
            return dataCount;
        }

        
        /// <summary>
        /// Finds all of the numbers at indexes separated by the interval.
        /// </summary>
        /// <returns>A list of all of the numbers from the index.</returns>
        public List<int> GetIntervals() {
            List<int> intervals = new List<int>();
            int intervalIndex = 0;
            while (intervalIndex < dataCount) {
                intervals.Add(data.ElementAt(intervalIndex));
                intervalIndex += dataInterval;
            }
            return intervals;
        }

        /// <summary>
        /// Sorts data in the data-collection into the given direction.
        /// </summary>
        /// <param name="direction">1: Ascending, -1: Descending</param>
        public void Sort(int direction) {
            List<int> sortedList = new List<int>();
            switch (dataCount) {
                case (256):
                    sortedList = MergeSort(data, direction);
                    break;
                case (2048):
                    sortedList = MergeSort(data, direction);
                    break;
                default:
                    Console.WriteLine("Default");
                    break;
            }
            data = sortedList;
        }

        /// <summary>
        /// Entry method to merge-sort. Calls itself recursively.
        /// </summary>
        /// <param name="list">The list of integers to sort.</param>
        /// <param name="direction">1: Ascending, -1: Descending</param>
        /// <returns>Sorted list from the given list.</returns>
        private List<int> MergeSort(List<int> list, int direction)
        {
            
            if (list.Count <= 1)
            {
                return list;
            }

            try
            {
                int midPoint = list.Count / 2;

                List<int> leftArray = list.GetRange(0, midPoint);
                // Right array can use midpoint as we know there's an even number of elements.
                List<int> rightArray = list.GetRange(midPoint, midPoint);

                List<int> sortedLeft = MergeSort(leftArray, direction);
                List<int> sortedRight = MergeSort(rightArray, direction);

                List<int> result = MergeLists(sortedLeft, sortedRight, direction);
                return result;


            }
            catch (Exception ex) {
                Console.WriteLine($"{ex}");
            }
            
            return list;
        }

        /// <summary>
        /// Merges two sorted lists into one.
        /// </summary>
        /// <param name="left">Sorted List</param>
        /// <param name="right">Sorted List</param>
        /// <param name="direction">1: Sort Ascending, -1: Sort Descending</param>
        /// <returns>A sorted union of the two lists.</returns>
        private List<int> MergeLists(List<int> left, List<int> right, int direction) {
            int leftCount = left.Count;
            int rightCount = right.Count;
            List<int> mergedList = new List<int>();
            
            while (leftCount != 0 && rightCount != 0)
            {
                if ((left.First() * direction) >= (right.First() * direction))
                {
                    mergedList.Add(right.First());
                    right.RemoveAt(0);
                    rightCount--;
                }
                else { 
                    mergedList.Add(left.First());
                    left.RemoveAt(0);
                    leftCount--;
                }
            }
            if (leftCount != 0) {
                mergedList.AddRange(left);
            }
            if (rightCount != 0) {
                mergedList.AddRange(right);
            }
            return mergedList;

        }

        public (int Number, int Count, int[] indexes) BinarySearch(int lowerBoundary, int upperBoundary, int searchValue, int direction) {
            
            int midPoint = (lowerBoundary + upperBoundary) / 2;
            int midPointValue = data.ElementAt(midPoint);

            Console.WriteLine($"UpperBound: {upperBoundary}, LowerBound: {lowerBoundary}, midPoint: {midPoint}, midPointValue: {midPointValue}");
            
            if (lowerBoundary >= (upperBoundary - 1)) {
                if (lowerBoundary == 0) {
                    return BinaryRange(data.First(), 1);
                }
                if (upperBoundary == data.Count) {
                    return BinaryRange(data.Last(), data.Count-1);
                }
                if ((midPointValue - data.ElementAt(midPoint - 1) * direction) > ((data.ElementAt(midPoint + 1) - midPoint) * direction))
                {
                    return BinaryRange(data.ElementAt(midPoint + 1), midPoint + 1);
                }
                else {
                    return BinaryRange(data.ElementAt(midPoint - 1), midPoint - 1);
                }
            }

            if (midPointValue == searchValue)
            {
                return BinaryRange(searchValue, midPoint);
            }
            else {
                if ((midPointValue * direction) > (searchValue * direction))
                {
                    return BinarySearch(lowerBoundary, midPoint - 1, searchValue, direction);
                }
                else { 
                    return BinarySearch(midPoint + 1, upperBoundary, searchValue, direction);
                }
            }

        }

        private (int Number, int Count, int[] indexes) BinaryRange(int searchValue, int index) {
            int lowerIndex = index;
            int upperIndex = index;
            while (data.ElementAt(lowerIndex) == searchValue && lowerIndex > 0){
                lowerIndex--;
            };
            while (data.ElementAt(upperIndex) == searchValue && upperIndex < data.Count - 1) {
                upperIndex++;
            }

            return (searchValue, (upperIndex - lowerIndex) - 1, Enumerable.Range(lowerIndex, upperIndex - lowerIndex).ToArray());
            
        }
    }
}
