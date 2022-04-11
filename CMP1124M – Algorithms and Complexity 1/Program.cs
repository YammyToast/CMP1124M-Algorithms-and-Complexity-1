using System;

namespace CMP1124M_Algorithms_and_Complexity_1

{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialises a list of the preset files.
            List<string> dataFileNames = new List<string>() {
                "Share_1_256", "Share_1_2048",
                "Share_2_256", "Share_2_2048",
                "Share_3_256", "Share_3_2048"
            };

            // Initialises a list to store all of the file-data.
            List<DataCollection> dataObjects = new List<DataCollection>();

            // Loads the file-data into the list using the preset file-names.
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

            State state = State.UserInput;
            string stateInput = " ";
            while (state == State.UserInput) {
                Console.WriteLine($"1. Analyse a file | 2. Merge a file | 3. Exit ");
                try
                {
                    Console.Write(" : ");
                    stateInput = Console.ReadLine();
                    state = (State)Enum.Parse(typeof(State), stateInput);

                    Console.WriteLine(state);

                    if (state == State.AnalyseFile)
                    {
                        Console.WriteLine($"| Loaded Files |");
                        foreach (string file in dataFileNames)
                        {
                            Console.WriteLine($" > {file}");
                        }
                        Console.WriteLine($"Please Choose a file");
                        try
                        {
                            Console.Write($" : ");
                            string fileInput = Console.ReadLine();
                            if (dataFileNames.Exists(x => x == fileInput))
                            {
                                DataCollection file = dataObjects.ElementAt(dataFileNames.IndexOf(fileInput));
                                AnalyseFile(file);
                            }
                            else
                            {
                                throw new Exception("Invalid File Name");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                    }
                    else {
                        // Else, continue looping.
                        state = State.UserInput;
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Invalid/No Argument Given");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid Input");
                }
                catch { 
                    
                }
            }
            

            //foreach (DataCollection dataCollection in dataObjects) {
            //    foreach(int enumVal in Enum.GetValues(typeof(Directions)))
            //    {
            //        TimeSpan totalTicks = TimeSpan.FromTicks(0);
            //        Output logger = new();


            //        DateTime startTime = DateTime.Now;

            //        dataCollection.Sort(enumVal);

            //        DateTime endTime = DateTime.Now;
            //        TimeSpan ticksTaken = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
            //        totalTicks += ticksTaken;
            //        logger.WriteSortResults(dataCollection, ticksTaken);





            //        startTime = DateTime.Now;

            //        (int Number, int[] indexes) searchResults = dataCollection.BinarySearch(0, dataCollection.getCount() - 1, searchValue, enumVal);
                    
            //        endTime = DateTime.Now;
            //        ticksTaken = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
            //        totalTicks += ticksTaken;
            //        logger.WriteSearchResults(searchResults, ticksTaken);

            //        List<int> intervals = dataCollection.FindIntervals();
            //        logger.WriteIntervals(intervals, dataCollection.getInterval());
            //        //Console.WriteLine($"\nBinary Search Results: {searchResults.Number}, Found: {searchResults.indexes.Length}");
            //        //foreach (int index in searchResults.indexes) {
            //        //    Console.WriteLine($"[>] : {index}");
            //        //}
            //    }
                
            //}
        }


        public static void AnalyseFile(DataCollection file) {
            SortTypes defaultSort  = SortTypes.MergeSort;
            Directions defaultDirection = Directions.Ascending;

            
            Console.WriteLine($"\n\n Analyse | {file.fileName} | {file.getCount()} \n\nAvailable Sorts: (Defaults: {defaultSort}, {defaultDirection})");
            foreach (SortTypes sort in Enum.GetValues(typeof(SortTypes))) {
                Console.Write($"| {sort} |");    
            }
            try
            {
                Console.Write("\n : ");
                //sortInput = Console.ReadLine();

                //state = (State)Enum.Parse(typeof(State), stateInput);
            }
            catch (Exception ex) { 
                
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

        enum State { 
            UserInput,
            AnalyseFile,
            MergeFile,
            Exit
        }
    }
        public enum Directions
        {
            Descending = -1,
            Ascending = 1,
        }
}