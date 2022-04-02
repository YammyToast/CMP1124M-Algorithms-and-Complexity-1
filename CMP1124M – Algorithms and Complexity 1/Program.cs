using System;

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

            foreach (DataCollection dataCollection in dataObjects) {
                foreach(int enumVal in Enum.GetValues(typeof(Directions)))
                {
                    DateTime startTime = DateTime.Now;

                    dataCollection.Sort(enumVal);

                    DateTime endTime = DateTime.Now;
                    TimeSpan ticksTaken = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
                    Console.WriteLine($"\n ─────────┤ Filename: {dataCollection.fileName} | Time Taken: > {ticksTaken.TotalMilliseconds}ms  |  Data Count: {dataCollection.getCount()} | Direction: {(Directions) enumVal} ├───────── \n");
                    foreach (int interval in dataCollection.GetIntervals()) {
                        Console.Write($" {interval} |");
                    }
                    Console.WriteLine($"\n");

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

        public enum Directions
        {
            Descending = -1,
            Ascending = 1,
        }

    }
}