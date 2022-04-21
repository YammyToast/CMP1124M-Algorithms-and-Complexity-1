using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class TestHandler
    {
        private List<DataCollection> dataCollections = new List<DataCollection>();
        public TestHandler(List<DataCollection> data) {
            dataCollections = data;
        }

        public void RunTests() {
            foreach (DataCollection collection in dataCollections) {
                foreach (SortTypes Sort in Enum.GetValues(typeof(SortTypes))) {
                    TimeSpan cumulativeTicks = TimeSpan.Zero;
                    int runs = 0;
                    for (int i = 0; i < 10000; i++)
                    {
                        DataCollection unsortedData = collection;
                        DateTime startTime = DateTime.Now;
                        unsortedData.Sort(Sort, Directions.Ascending);
                        DateTime endTime = DateTime.Now;
                        cumulativeTicks += TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                        runs++;

                    }
                    Console.WriteLine($"FileName: {collection.fileName}, Sort: {Sort}, Average Time: {cumulativeTicks.TotalMilliseconds / runs}");
                }
                
                
            }

        }

        public void RunSearch() { 
            
        }
    }
}
