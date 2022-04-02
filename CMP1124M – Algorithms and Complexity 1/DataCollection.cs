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

        public int getCount() {
            return dataCount;
        }

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
        /// Sorts data in the data-collection into the given direction.
        /// </summary>
        /// <param name="direction">0: Ascending, 1: Descending</param>
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

        public List<int> GetIntervals() {
            List<int> intervals = new List<int>();
            int intervalIndex = 0;
            while (intervalIndex < dataCount) {
                intervals.Add(data.ElementAt(intervalIndex));
                intervalIndex += dataInterval;
            }
            return intervals;
        }
    }
}
