﻿using System;

namespace CMP1124M_Algorithms_and_Complexity_1

{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> dataFileNames = new List<string>() {
                "Share_1_256", "Share_1_2048",
                "Share_2_256", "Share_2_2048",
                "Share_3_256", "Share_3_2048"
            };

            List<DataCollection> dataObjects = new List<DataCollection>();

            try
            {
                foreach (string file in dataFileNames)
                {
                    List<int> dataList = ReadData(file);
                    DataCollection dataCollection = new DataCollection(dataList, file);
                    dataObjects.Add(dataCollection);

                }
                if (dataObjects.Count != dataFileNames.Count) {
                    throw new Exception($"{dataObjects.Count}/{dataFileNames.Count}");
                }

            }
            catch (Exception ex) {
                Console.WriteLine($"Could not create all data collections: {ex}");
            }

            int searchValue = 100;
            
            foreach (DataCollection dataCollection in dataObjects) {
                foreach(int enumVal in Enum.GetValues(typeof(Directions)))
                {
                    TimeSpan totalTicks = TimeSpan.FromTicks(0);
                    Output logger = new();


                    DateTime startTime = DateTime.Now;

                    dataCollection.Sort(enumVal);

                    DateTime endTime = DateTime.Now;
                    TimeSpan ticksTaken = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                    totalTicks += ticksTaken;
                    logger.WriteSortResults(dataCollection, ticksTaken);


                    startTime = DateTime.Now;

                    List<int> intervals = dataCollection.GetIntervals();

                    endTime = DateTime.Now;
                    ticksTaken = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                    totalTicks += ticksTaken;
                    logger.WriteIntervals(intervals, dataCollection.getInterval(), ticksTaken);

                    startTime = DateTime.Now;

                    (int Number, int[] indexes) searchResults = dataCollection.BinarySearch(0, dataCollection.getCount() - 1, searchValue, enumVal);
                    
                    
                    Console.WriteLine($"\nBinary Search Results: {searchResults.Number}, Found: {searchResults.indexes.Length}");
                    foreach (int index in searchResults.indexes) {
                        Console.WriteLine($"[>] : {index}");
                    }
                }
                
            }
        }

        public static List<int> ReadData(string fileName) {
            
            List<int> dataList = new List<int>();
            int lineNumber = 1;
            try
            {
                string localFileDir = Environment.CurrentDirectory;
                string[] subs = localFileDir.Split(@"\bin");
                string fileDir = Path.Combine(subs[0], @"data\" + fileName + @".txt");
                string[] data = File.ReadAllText(fileDir).Split("\n");
                
                foreach (string s in data) {
                    int num;
                    if (int.TryParse(s, out num))
                    {
                        dataList.Add(num);
                        lineNumber++;
                    }
                    else {
                        throw new ArrayTypeMismatchException(s);
                    }
                }
                
            }
            catch (FileNotFoundException ex) {
                Console.WriteLine($"Could not find file: {fileName}, \n{ex}");
                Console.ReadKey();
                Environment.Exit(1);
            }
            catch (ArrayTypeMismatchException ex)
            {
                Console.WriteLine($"Nonconvertible data: {ex.Message.Split("\r")[0]} ({fileName} Line: {lineNumber}) ");
                Console.ReadKey();
                Environment.Exit(1);
            }
            

            return dataList;
        }


    }
        public enum Directions
        {
            Descending = -1,
            Ascending = 1,
        }
}