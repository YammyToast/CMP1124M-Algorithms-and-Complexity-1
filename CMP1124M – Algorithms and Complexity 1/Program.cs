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
                    DataCollection dataCollection = new DataCollection(dataList);
                    dataObjects.Add(dataCollection);

                }
                if (dataObjects.Count != dataFileNames.Count) {
                    throw new Exception($"{dataObjects.Count}/{dataFileNames.Count}");
                }
                Console.WriteLine(dataObjects.Count);
            }
            catch (Exception ex) {
                Console.WriteLine($"Could not create all data collections: {ex}");
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
                    if (int.TryParse(s, out num) == true)
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
                Console.Write($"Nonconvertible data: {ex.Message} ");
                //Console.Write($"({fileName} Line: {lineNumber})");
                Console.ReadKey();
                Environment.Exit(1);
            }


            return dataList;
        }
    }
}