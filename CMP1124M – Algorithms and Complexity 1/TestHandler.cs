﻿using System;
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
                    long avgSteps = 0;
                    for (int i = 0; i < 10000; i++)
                    {
                        List<int> dataCopy = new List<int>();
                        foreach (int data in collection.getData()) { 
                            dataCopy.Add(data);
                        }
                        DataCollection unsortedData = new DataCollection(dataCopy, collection.fileName);
                        DateTime startTime = DateTime.Now;
                        unsortedData.Sort(Sort, Directions.Ascending);
                        DateTime endTime = DateTime.Now;
                        cumulativeTicks += TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                        avgSteps += unsortedData.steps;
                        runs++;

                    }
                    Console.WriteLine($"FileName: {collection.fileName}, Sort: {Sort}, Steps: {avgSteps / runs}, Average Time: {cumulativeTicks.TotalMilliseconds / runs}");
                }
                
                
            }

        }

        public void RunSearch() {
            foreach (DataCollection collection in dataCollections)
            {
                
                    SearchTypes Search = SearchTypes.Jump;
                    collection.Sort(SortTypes.Merge, Directions.Ascending);
                    TimeSpan cumulativeTicks = TimeSpan.Zero;
                    int runs = 0;
                    long avgSteps = 0;
                    for (int i = 0; i < 10000; i++) {
                        Random rand = new Random();
                        int searchValue = rand.Next(collection.getData().First(), collection.getData().Last());

                        DateTime startTime = DateTime.Now;
                        collection.Search(Search, searchValue, (int) Directions.Ascending);
                        DateTime endTime = DateTime.Now;
                        cumulativeTicks += TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                        avgSteps += collection.steps;
                        runs++;
                    }
                    Console.WriteLine($"FileName: {collection.fileName}, Search: {Search}, Steps: {avgSteps / runs}, Average Time: {cumulativeTicks.TotalMilliseconds / runs}");
                
            }
        }

    }
}
