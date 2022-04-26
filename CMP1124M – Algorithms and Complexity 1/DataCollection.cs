using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class DataCollection
    {
        // Initialisation of class attributes.
        private List<int> data;
        private int dataCount;
        private int dataInterval;
        public readonly string fileName;
        public int sortDirection;
        private SortTypes sortUsed;

        public long steps = 0;

        // Constructor for class of empty data.
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
        // Constructor for class with given data.
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
            // While the next interval is valid, 
            // record that interval and progress.
            while ((intervalIndex + dataInterval) < dataCount - 1) {
                intervals.Add(data.ElementAt(intervalIndex));
                intervalIndex += dataInterval;
            }
            return intervals;
        }






        /// <summary>
        /// Sorts data in the data-collection into the given direction with given sort.
        /// </summary>
        /// <param name="direction">1: Ascending, -1: Descending</param>
        public void Sort(SortTypes sort, Directions direction) {
            // Saves the sort used, and the direction as attributes of the datacollection.
            sortDirection = (int) direction;
            sortUsed = (SortTypes) sort;

            steps = 0;


            List<int> sortedList = new List<int>();

            // Switch for running the different sort functions.
            switch (sort) {
                case SortTypes.Merge:
                    sortedList = MergeSort(data, sortDirection);
                    break;
                case SortTypes.Quick:
                    // As quicksort is static, I felt that it needed its own class.
                    QuickSort quickSort = new QuickSort(data, sortDirection);
                    sortedList = quickSort.getData();
                    steps = quickSort.getSteps();
                    break;
                case SortTypes.Heap:
                    sortedList = HeapSort(data, dataCount, sortDirection);
                    break;
                case SortTypes.Insertion:
                    sortedList = InsertionSort(data, sortDirection);
                    
                    break;
                // Default to using mergesort
                default:
                    sortedList = MergeSort(data, sortDirection);
                    break;
            }

            
            data = sortedList;
        }

        /// <summary>
        /// Searches list of sorted data in the given direction with the given search.
        /// </summary>
        /// <param name="search">Search type to use</param>
        /// <param name="searchValue">Value to search for.</param>
        /// <param name="direction">Direction the data is sorted in. 1: Ascending, -1: Descending</param>
        /// <returns></returns>
        public (int Number, int[] indexes) Search(SearchTypes search, int searchValue, int direction) {

            steps = 0;

            // Initialise tuple for the search results.
            (int Number, int[] indexes) searchResults = (searchValue, new int[1] { -1 });
            // Switch for running the different search functions.
            switch (search) {
                case SearchTypes.Binary:
                    searchResults = BinarySearch(0, data.Count - 1, searchValue, direction);
                    break;
                case SearchTypes.Jump:
                    searchResults = JumpSearch(searchValue, direction);
                    break;
                // Default to using binary search
                default:
                    searchResults = BinarySearch(0, data.Count - 1, searchValue, direction);
                    break;

            }

            return searchResults;
        }







        /// <summary>
        /// Entry method to merge-sort. Calls itself recursively.
        /// </summary>
        /// <param name="list">The list of integers to sort.</param>
        /// <param name="direction">1: Ascending, -1: Descending</param>
        /// <returns>Sorted list from the given list.</returns>
        private List<int> MergeSort(List<int> list, int direction)
        {
            // If only one element is in the given list, return one step backwards.
            if (list.Count <= 1)
            {
                return list;
            }

            try
            {
                int midPoint = list.Count / 2;

                // Split list into 2, around the midpoint.
                List<int> leftArray = list.GetRange(0, midPoint);
                steps++;
                // Right array can use midpoint as we know there's an even number of elements.
                List<int> rightArray = list.GetRange(midPoint, midPoint);
                steps++;

                // PseudoCode
                // Recursively call this function, splitting the data in halves.
                // Final items on stack are functions with list parameters of one element.
                // Gradually pair together lists from the stack until stack is empty.

                // Sort the left and right splits.
                List<int> sortedLeft = MergeSort(leftArray, direction);
                
                List<int> sortedRight = MergeSort(rightArray, direction);
                
                // Merge the results of the sorted splits into one sorted list.
                List<int> result = MergeLists(sortedLeft, sortedRight, direction);
                steps++;
                // Return the merged, sorted splits.
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
            steps++;
            int rightCount = right.Count;
            steps++;
            List<int> mergedList = new List<int>();
            //While there are still elements in either of the lists.
            while (leftCount != 0 && rightCount != 0)
            {
                // Sort first elements of each list.
                // Once item has been sorted, remove from the front of its list.
                if ((left.First() * direction) >= (right.First() * direction))
                {
                    mergedList.Add(right.First());
                    steps++;
                    right.RemoveAt(0);
                    steps++;
                    rightCount--;
                }
                else { 
                    mergedList.Add(left.First());
                    steps++;
                    left.RemoveAt(0);
                    steps++;
                    leftCount--;
                }
            }
            // If all of the elements in one stack have been removed,
            // -concat the remaining items in the remaining list.
            if (leftCount != 0) {
                mergedList.AddRange(left);
                steps++;
            }
            if (rightCount != 0) {
                mergedList.AddRange(right);
                steps++;
            }
            return mergedList;

        }

        /// <summary>
        /// Header method for Heap sort
        /// </summary>
        /// <param name="list">List of values to be sorted</param>
        /// <param name="count">The amount of values in the given list.</param>
        /// <param name="direction">The direction to sort in. 1: Ascending, -1: Descending </param>
        /// <returns>List of sorted values.</returns>
        private List<int> HeapSort(List<int> list, int count, int direction) {
            // Consecutively build heaps for the height of the tree.
            // The list is composed of unsorted values tree nodes,
            // and a partitioned set of the largest values at the end,
            // trailed by the partition tail.
            for (int partitionTail = (count / 2) - 1; partitionTail >= 0; partitionTail--) {
                list = BuildHeap(list, count, partitionTail, direction);
                steps++;
            }
            // Consecutively build heaps for the amount of items in the tree.
            for (int i = count - 1; i >= 0; i--) {
                // Switch the first and indexed element so new element is at the top.
                int temp = list.ElementAt(0);
                list[0] = list.ElementAt(i);
                list[i] = temp;
                steps++;
                // Build a heap from the new list.
                list = BuildHeap(list, i, 0, direction);
            }
            return list;
        }

        private List<int> BuildHeap(List<int> list, int count, int partitionTail, int direction) {
            
            int leftBoundary = (partitionTail * 2) + 1;

            int rightBoundary = (partitionTail * 2) + 2;


            int largestIndex = partitionTail;

            // If a new largest value is found at either of the tree-traversers, move that value to the top of the tree.
            if (leftBoundary < count && (list.ElementAt(leftBoundary) * direction) > (list.ElementAt(largestIndex) * direction)) {
                largestIndex = leftBoundary;
            }
            if (rightBoundary < count && (list.ElementAt(rightBoundary) * direction) > (list.ElementAt(largestIndex) * direction)) {
                largestIndex = rightBoundary;
            }
            // If an element has been changed:
            if (largestIndex != partitionTail) {
                // Move the value at the top of the tree to the end of the partition.
                int temp = list.ElementAt(largestIndex);
                list[largestIndex] = list.ElementAt(partitionTail);
                list[partitionTail] = temp;
                steps++;
                // Build a new heap excluding the newly partitioned value.
                list = BuildHeap(list, count, largestIndex, direction);
            }

            // Return the sorted list.
            return list;
      
        }

        /// <summary>
        /// Function for Insertion sort algorithm.
        /// </summary>
        /// <param name="list">List of values to be sorted.</param>
        /// <param name="direction">The direction to sort them in.</param>
        /// <returns>List of sorted values.</returns>
        private List<int> InsertionSort(List<int> list, int direction) {
            // For each element in the list (excluding the first).
            for (int index = 1; index < list.Count; index++) { 
                // Save the value of the index that is going to be moved.
                int moveValue = list[index];
                
                // Set the search index to the index below the move-value,
                // (this index being the end of the sorted partition).
                int searchIndex =  index - 1;

                // While the moveValue is not in its correct place in the sorted partition,
                // keep searching deeper.
                while (searchIndex >= 0 && (moveValue * direction) < (list[searchIndex] * direction)) {
                    // Move all of the values up, as to make space for the index, once its position is found.
                    list[searchIndex + 1] = list[searchIndex];
                    steps++;
                    searchIndex--;
                }
                // Once location is found,
                // set that index to the move value.
                list[searchIndex + 1] = moveValue;
                steps++;
            }
            return list;
        }

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
            steps++;
            int midPointValue = data.ElementAt(midPoint);

            // If the value at the midpoint is the search-value, return the range of indexes.
            if (searchValue == midPointValue) {
                steps++;
                return GetSortedRange(searchValue, midPoint);
            }

            // If the upper and lower boundaries are 1 index apart, and the midpoint value is not equal to the-
            // search value, the value must not exist in the sorted list.
            if (upperBoundary - 1 == lowerBoundary) {
                // Determine the closest value to the search value.
                int closestLower = Math.Abs((midPointValue * direction) - data.ElementAt(lowerBoundary));
                steps++;
                int closestUpper = Math.Abs((midPointValue * direction) - data.ElementAt(upperBoundary));
                steps++;
                // Get the range of indexes, instead using the closest value.
                if (closestLower <= closestUpper)
                {
                    return GetSortedRange(data.ElementAt(lowerBoundary), lowerBoundary);
                }
                else { 
                    return GetSortedRange(data.ElementAt(upperBoundary), upperBoundary);
                }

            }

            // Recursive binary search calls for narrowing down the index.
            if ((searchValue * direction) > (midPointValue * direction)) {
                steps++;
                return BinarySearch(midPoint, upperBoundary, searchValue, direction);
            }
            else {
                steps++;
                return BinarySearch(lowerBoundary, midPoint, searchValue, direction);
            }

        }

        /// <summary>
        /// Function for the jump search algorithm.
        /// </summary>
        /// <param name="searchValue">Value to search for.</param>
        /// <param name="direction">The direction in which the list is sorted.</param>
        /// <returns>Tuple of the number searched for, and the indexes at which it is found.</returns>
        private (int Number, int[] indexes) JumpSearch(int searchValue, int direction) {
            // Determine which half of the list the value is located in, and set as the starting location.
            int searchingIndex = (searchValue < data.ElementAt(dataCount / 2)) ? 0 : dataCount / 2;
            // Set the step-interval.
            int stepInterval = (int)Math.Sqrt(dataCount);
            steps++;
            try
            {
                // While the search value is not located in the current interval, continue to the next interval.
                while (searchingIndex <= dataCount - stepInterval && data.ElementAt(searchingIndex + stepInterval) < searchValue)
                {
                    searchingIndex += stepInterval;
                    steps++;
                }

            }
            catch (Exception ex) { 
                
            }
            // Once the search-value is known to be in an interval.
            // Perform a linear search to find the index in that interval.
            while (searchingIndex <= dataCount && data.ElementAt(searchingIndex) < searchValue)
            {
                searchingIndex++;
                steps++;
            }
            // If the value can be found in the interval, get its indexes.
            if (data.ElementAt(searchingIndex) == searchValue)
            {
                return GetSortedRange(searchValue, searchingIndex);
            }
            // If the value is not in the sorted list.
            else {
                // Determine the closest value to the search value.
                int closestLower = Math.Abs((searchValue * direction) - data.ElementAt(searchingIndex - 1));
                steps++;
                int closestUpper = Math.Abs((searchValue * direction) - data.ElementAt(searchingIndex));
                steps++;
                // Get the range of indexes, instead using the closest value.
                if (closestLower <= closestUpper)
                {
                    return GetSortedRange(data.ElementAt(searchingIndex - 1), searchingIndex - 1);
                }
                else
                {
                    return GetSortedRange(data.ElementAt(searchingIndex), searchingIndex);
                }
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
                // While the searchValue is still being found at the search boundaries,
                // increment in corresponding direction, and increase number of occurences found.
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

            // Return the value (or closest found), and the range between the two boundaries as the indexes.

            return (searchValue, Enumerable.Range(lowerSearchBoundary + 1, count).ToArray());
        }





    }
    public enum SortTypes { 
        Merge = 1,
        Quick,
        Heap,
        Insertion
    }

    public enum SearchTypes { 
        Binary = 1,
        Interpolation,
        Jump
    }
}
