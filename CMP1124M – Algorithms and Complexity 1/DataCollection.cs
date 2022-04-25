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

        public long steps = 0;


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
            while ((intervalIndex + dataInterval) < dataCount - 1) {
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

            steps = 0;


            List<int> sortedList = new List<int>();

            switch (sort) {
                case SortTypes.Merge:
                    sortedList = MergeSort(data, sortDirection);
                    break;
                case SortTypes.Quick:
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

                default:
                    sortedList = MergeSort(data, sortDirection);
                    break;
            }

            
            data = sortedList;
        }

        public (int Number, int[] indexes) Search(SearchTypes search, int searchValue, int direction) {

            steps = 0;

            (int Number, int[] indexes) searchResults = (searchValue, new int[1] { -1 });
            switch (search) {
                case SearchTypes.Binary:
                    searchResults = BinarySearch(0, data.Count - 1, searchValue, direction);
                    break;
                case SearchTypes.Jump:
                    searchResults = JumpSearch(searchValue, direction);
                    break;
                //case SearchTypes.Interpolation:
                //    searchResults = InterpolationSearch(0, data.Count - 1, searchValue, direction);
                //    break;
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
            
            if (list.Count <= 1)
            {
                return list;
            }

            try
            {
                int midPoint = list.Count / 2;

                List<int> leftArray = list.GetRange(0, midPoint);
                steps++;
                // Right array can use midpoint as we know there's an even number of elements.
                List<int> rightArray = list.GetRange(midPoint, midPoint);
                steps++;

                // PseudoCode
                // Recursively call this function, splitting the data in halves.
                // Final items on stack are functions with list parameters of one element.
                // Gradually pair together lists from the stack until stack is empty.

                List<int> sortedLeft = MergeSort(leftArray, direction);
                
                List<int> sortedRight = MergeSort(rightArray, direction);
                
                List<int> result = MergeLists(sortedLeft, sortedRight, direction);
                steps++;
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
            
            while (leftCount != 0 && rightCount != 0)
            {
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
            // -concat the remaining items in the remaining list directly.
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

        
        private List<int> HeapSort(List<int> list, int count, int direction) {
            for (int partitionTail = (count / 2) - 1; partitionTail >= 0; partitionTail--) {
                list = BuildHeap(list, count, partitionTail, direction);
                steps++;
            }
            for (int i = count - 1; i >= 0; i--) {
                int temp = list.ElementAt(0);
                list[0] = list.ElementAt(i);
                list[i] = temp;
                steps++;
                list = BuildHeap(list, i, 0, direction);
            }
            return list;
        }

        private List<int> BuildHeap(List<int> list, int count, int partitionTail, int direction) {
            
            int leftBoundary = (partitionTail * 2) + 1;

            int rightBoundary = (partitionTail * 2) + 2;


            int largestIndex = partitionTail;

            if (leftBoundary < count && (list.ElementAt(leftBoundary) * direction) > (list.ElementAt(largestIndex) * direction)) {
                largestIndex = leftBoundary;
            }
            if (rightBoundary < count && (list.ElementAt(rightBoundary) * direction) > (list.ElementAt(largestIndex) * direction)) {
                largestIndex = rightBoundary;
            }
            // i.e if an element has been changed.
            if (largestIndex != partitionTail) {
                int temp = list.ElementAt(largestIndex);
                list[largestIndex] = list.ElementAt(partitionTail);
                list[partitionTail] = temp;
                steps++;
                list = BuildHeap(list, count, largestIndex, direction);
            }


            

            return list;
      
        }

        private List<int> InsertionSort(List<int> list, int direction) {
            for (int index = 1; index < list.Count; index++) { 
                int moveValue = list[index];
                
                int searchIndex =  index - 1;

                while (searchIndex >= 0 && (moveValue * direction) < (list[searchIndex] * direction)) {
                    list[searchIndex + 1] = list[searchIndex];
                    steps++;
                    searchIndex--;
                }
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

            if (searchValue == midPointValue) {
                steps++;
                return GetSortedRange(searchValue, midPoint);
            }

            if (upperBoundary - 1 == lowerBoundary) {
                int closestLower = Math.Abs((midPointValue * direction) - data.ElementAt(lowerBoundary));
                steps++;
                int closestUpper = Math.Abs((midPointValue * direction) - data.ElementAt(upperBoundary));
                steps++;
                if (closestLower <= closestUpper)
                {
                    return GetSortedRange(data.ElementAt(lowerBoundary), lowerBoundary);
                }
                else { 
                    return GetSortedRange(data.ElementAt(upperBoundary), upperBoundary);
                }

            }

            if ((searchValue * direction) > (midPointValue * direction)) {
                steps++;
                return BinarySearch(midPoint, upperBoundary, searchValue, direction);
            }
            else {
                steps++;
                return BinarySearch(lowerBoundary, midPoint, searchValue, direction);
            }

        }

        private (int Number, int[] indexes) JumpSearch(int searchValue, int direction) {
            int searchingIndex = (searchValue < data.ElementAt(dataCount / 2)) ? 0 : dataCount / 2;
            int stepInterval = (int)Math.Sqrt(dataCount);
            steps++;
            try
            {
                while (searchingIndex <= dataCount - stepInterval && data.ElementAt(searchingIndex + stepInterval) < searchValue)
                {
                    searchingIndex += stepInterval;
                    steps++;
                }

            }
            catch (Exception ex) { 
                
            }
            while (searchingIndex <= dataCount && data.ElementAt(searchingIndex) < searchValue)
            {
                searchingIndex++;
                steps++;
            }
            if (data.ElementAt(searchingIndex) == searchValue)
            {
                return GetSortedRange(searchValue, searchingIndex);
            }
            else {
                int closestLower = Math.Abs((searchValue * direction) - data.ElementAt(searchingIndex - 1));
                steps++;
                int closestUpper = Math.Abs((searchValue * direction) - data.ElementAt(searchingIndex));
                steps++;
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

        //private (int Number, int[] indexes) InterpolationSearch(int lowerBoundary, int upperBoundary, int searchValue, int direction) {
            
        //    if (data.ElementAt(0) > searchValue) return GetSortedRange(data.ElementAt(0), 0);
        //    if (data.ElementAt(dataCount - 1) < searchValue) return GetSortedRange(data.ElementAt(dataCount - 1), dataCount - 1);
        //    if (lowerBoundary == upperBoundary) return GetSortedRange(data.ElementAt(lowerBoundary), lowerBoundary);

        //    int lowerVal = data.ElementAt(lowerBoundary);
        //    int upperVal = data.ElementAt(upperBoundary);

        //    try
        //    {
        //        //Console.Write($"search: {searchValue}, lower: {lowerBoundary} => {data.ElementAt(lowerBoundary)}, upper: {upperBoundary} => {data.ElementAt(upperBoundary)} ");
        //        if ((searchValue * direction) >= (data.ElementAt(lowerBoundary) * direction) && (searchValue * direction) <= (data.ElementAt(upperBoundary) * direction) && lowerBoundary <= upperBoundary)
        //        {


        //            //int interpolationPosition = (lowerBoundary + ((searchValue - data.ElementAt(lowerBoundary)) * (upperBoundary - lowerBoundary))) / (data.ElementAt(upperBoundary) - data.ElementAt(lowerBoundary));
        //            int interpolationPosition = lowerBoundary + (((upperBoundary - lowerBoundary) / (data.ElementAt(upperBoundary) - data.ElementAt(lowerBoundary))) * (searchValue - data.ElementAt(lowerBoundary)));

        //            //Console.Write($"interpolation: {interpolationPosition}, interpolationVal: {data.ElementAt(interpolationPosition)} \n");
        //            if (data.ElementAt(interpolationPosition) == searchValue)
        //            {

        //                return GetSortedRange(searchValue, interpolationPosition);
        //            }
        //            else if ((data.ElementAt(interpolationPosition) * direction) > (searchValue * direction))
        //            {
        //                return InterpolationSearch(lowerBoundary, interpolationPosition - 1, searchValue, direction);
        //            }
        //            else
        //            {
        //                return InterpolationSearch(interpolationPosition + 1, upperBoundary, searchValue, direction);
        //            }
        //        }
        //    }
        //    catch (Exception ex) { 
            
        //    }

        //    //Console.WriteLine($"Search value doesn't exist in data set");
        //    try
        //    {

        //        if ((searchValue * direction) >= (data.ElementAt(lowerBoundary - 1) * direction) && (searchValue * direction) <= (data.ElementAt(lowerBoundary) * direction))
        //        {

        //            int closestLower = Math.Abs((searchValue * direction) - data.ElementAt(lowerBoundary - 1));
        //            int closestUpper = Math.Abs((searchValue * direction) - data.ElementAt(lowerBoundary));
        //            if (closestLower <= closestUpper)
        //            {
        //                return GetSortedRange(data.ElementAt(lowerBoundary - 1), lowerBoundary - 1);
        //            }
        //            else
        //            {
        //                return GetSortedRange(data.ElementAt(lowerBoundary), lowerBoundary);
        //            }

        //        }
        //    }
        //    catch (Exception ex) { 
                
        //    }
        //    // Something is wrong if it gets to here.
        //    return (searchValue, new int[0]);
        //}


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
