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
        public int sortDirection;
        private SortTypes sortUsed;


        public DataCollection(string _fileName) {
            fileName = _fileName;
            
            if (dataCount <= 256)
            {
                dataInterval = 10;
            }
            else { 
                dataInterval = 50;
            }
        }

        public DataCollection(List<int> _data, string _fileName) : this(_fileName) { 
            data = _data;
            dataCount = _data.Count;
            
        }

        public List<int> getData() {
            return data;
        }

        public void setData(List<int> _data) {
            data = _data;
            dataCount = _data.Count;
        }

        /// <summary>
        /// Gets the amount of data values in the data collection.
        /// </summary>
        /// <returns></returns>
        public int getCount() {
            return dataCount;
        }

        public int getInterval() {
            return dataInterval;
        }

        public SortTypes getSortUsed() {
            return sortUsed;
        }

        

        
        /// <summary>
        /// Finds all of the numbers at indexes separated by the interval.
        /// </summary>
        /// <returns>A list of all of the numbers from the index.</returns>
        public List<int> FindIntervals() {
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
        public void Sort(SortTypes sort, Directions direction) {
            // Saves the sort used, and the direction as attributes of the datacollection.
            sortDirection = (int) direction;
            sortUsed = (SortTypes) sort;


            List<int> sortedList = new List<int>();

            switch (sort) {
                case SortTypes.MergeSort:
                    sortedList = MergeSort(data, sortDirection);
                    break;
                case SortTypes.QuickSort:
                    QuickSort quickSort = new QuickSort(data, sortDirection);
                    sortedList = quickSort.getData();
                    
                    break;

                default:
                    sortedList = MergeSort(data, sortDirection);
                    break;
            }

            sortedList = MergeSort(data, sortDirection);
            
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

                // PseudoCode
                // Recursively call this function, splitting the data in halves.
                // Final items on stack are functions with list parameters of one element.
                // Gradually pair together lists from the stack until stack is empty.

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
        public List<int> MergeLists(List<int> left, List<int> right, int direction) {
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
            // If all of the elements in one stack have been removed,
            // -concat the remaining items in the remaining list directly.
            if (leftCount != 0) {
                mergedList.AddRange(left);
            }
            if (rightCount != 0) {
                mergedList.AddRange(right);
            }
            return mergedList;

        }

        //private static void QuickSort(List<int> list, int lowerBoundary, int upperBoundary, int direction) {
        //    int lowerIndex = lowerBoundary + 1;
        //    int upperIndex = upperBoundary;

        //    int pivotNum = list.ElementAt(lowerBoundary);

        //    while (lowerIndex <= upperIndex) {
        //        while (lowerIndex <= upperIndex) {
        //            while ((list.ElementAt(lowerIndex) * direction) < (pivotNum * direction)) {
        //                lowerIndex++;
        //            }
        //            while ((list.ElementAt(upperIndex) * direction) > (pivotNum * direction)) { 
        //                upperIndex--;
        //            }

        //            if (lowerIndex <= upperIndex) {
        //                int temp = list.ElementAt(lowerIndex);
        //                list[lowerIndex] = list.ElementAt(upperIndex);
        //                list[upperIndex] = temp;

        //            }
        //        }
        //    }
        //    if (lowerBoundary < upperIndex) {
        //        QuickSort(list, lowerBoundary, upperIndex, direction);
        //    }

        //    if (lowerIndex < upperBoundary) { 
        //        QuickSort(list, lowerIndex, upperBoundary, direction);  
        //    }

        //}

        


        /// <summary>
        /// Performs a binary search on the ordered list, given the direction the list is sorted in.
        /// Recursive Algorithm.
        /// </summary>
        /// <param name="lowerBoundary">The lower boundary of the search space.</param>
        /// <param name="upperBoundary">The upper boundary of the search space.</param>
        /// <param name="searchValue">The value to search the list for.</param>
        /// <param name="direction">The direction in which the list is sorted.
        /// Ascending, Descending.
        /// </param>
        /// <returns>Tuple of the number searched for, and the indexes at which it is found.</returns>
        public (int Number, int[] indexes) BinarySearch(int lowerBoundary, int upperBoundary, int searchValue, int direction) {
            
            int midPoint = (lowerBoundary + upperBoundary) / 2;
            int midPointValue = data.ElementAt(midPoint);

            if (searchValue == midPointValue) {

                return GetSortedRange(searchValue, midPoint);
            }

            if (upperBoundary - 1 == lowerBoundary) {
                int closestLower = Math.Abs((midPointValue * direction) - data.ElementAt(lowerBoundary));
                int closestUpper = Math.Abs((midPointValue * direction) - data.ElementAt(upperBoundary));
                if (closestLower <= closestUpper)
                {
                    return GetSortedRange(data.ElementAt(lowerBoundary), lowerBoundary);
                }
                else { 
                    return GetSortedRange(data.ElementAt(upperBoundary), upperBoundary);
                }

            }

            if ((searchValue * direction) > (midPointValue * direction)) {
                return BinarySearch(midPoint, upperBoundary, searchValue, direction);
            }
            else {
                return BinarySearch(lowerBoundary, midPoint, searchValue, direction);
            }

        }

        /// <summary>
        /// Finds the count of neighbouring values surrounding the given index and value to search for in the list. 
        /// Requires the list to be sorted.
        /// </summary>
        /// <param name="searchValue">Value to filter for.</param>
        /// <param name="index">The start index to search around.</param>
        /// <returns>Tuple of the number searched for, and the indexes at which it is found.</returns>
        private (int Number, int[] indexes) GetSortedRange(int searchValue, int index) {


            int count = 1;
            int upperSearchBoundary = index + 1, lowerSearchBoundary = index - 1;
            
            try
            {

                while (data.ElementAt(upperSearchBoundary) == searchValue && upperSearchBoundary < data.Count)
                {

                    upperSearchBoundary++; count++;
                    if (upperSearchBoundary == data.Count) {
                        break;
                    }

                }
                while (data.ElementAt(lowerSearchBoundary) == searchValue && lowerSearchBoundary > 0)
                {

                    lowerSearchBoundary--; count++;
                    if (lowerSearchBoundary < 0)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            // This works somehow, and I don't want to touch it.

            return (searchValue, Enumerable.Range(lowerSearchBoundary + 1, count).ToArray());
        }

    }
    public enum SortTypes { 
        MergeSort = 1,
        QuickSort
    }

    public enum SearchTypes { 
        BinarySearch = 1,
    }
}
