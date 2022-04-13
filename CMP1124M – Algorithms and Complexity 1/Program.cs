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


            int mergedFileCounter = 0;

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
                            Console.WriteLine($"{ex}");
                        }
                        state = State.UserInput;


                    } else if (state == State.MergeFile)
                    {
                        string leftFileName = string.Empty;
                        string rightFileName = string.Empty;
                        try 
                        {
                            while (!dataFileNames.Exists(x => x == leftFileName)) {
                                Console.WriteLine($"| Loaded Files |");
                                foreach (string file in dataFileNames)
                                {
                                    Console.WriteLine($" > {file}");
                                }
                                Console.WriteLine($"Choose the first file in the merge: \n\n");
                                Console.Write(" : ");
                                leftFileName = Console.ReadLine();
                            }
                            while (!dataFileNames.Exists(x => x == rightFileName))
                            {
                                Console.WriteLine($"| Loaded Files |");
                                foreach (string file in dataFileNames)
                                {
                                    Console.WriteLine($" > {file}");
                                }
                                Console.WriteLine($"Choose the second file in the merge: \n\n");
                                Console.Write(" : ");
                                rightFileName = Console.ReadLine();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        DataCollection mergedFile = MergeFiles(dataObjects.ElementAt(dataFileNames.IndexOf(leftFileName)),
                                                                dataObjects.ElementAt(dataFileNames.IndexOf(rightFileName)));
                        dataObjects.Add(mergedFile);
                        string fileSuffix = (mergedFileCounter > 0) ? $"({mergedFileCounter})" : "";
                        dataFileNames.Add($"{mergedFile.fileName}{fileSuffix}");
                        mergedFileCounter++;
                        state = State.UserInput;
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
            SortTypes chosenSort  = SortTypes.MergeSort;
            Directions chosenDirection = Directions.Ascending;
            string sortInput = string.Empty;
            string directionInput = string.Empty;
            string searchString = string.Empty;
            int searchValue = 0;
            
            Console.WriteLine($"\n\n Analyse | {file.fileName} | {file.getCount()} \n\nAvailable Sorts: (Defaults: {chosenSort}, {chosenDirection})");
            // Inputting is wrapped in a try clause to handle null-arguments.
            try
            {
                // === Sort Selection ===
                foreach (SortTypes sort in Enum.GetValues(typeof(SortTypes)))
                {
                    Console.Write($"| {sort} |");
                }
                Console.Write("\n : ");
                sortInput = Console.ReadLine();
                foreach (SortTypes checkSort in Enum.GetValues(typeof(SortTypes)))
                {
                    if (sortInput.ToLower() == checkSort.ToString().ToLower())
                    {
                        chosenSort = checkSort;
                    }
                }
                Console.WriteLine($"Using Sort: {chosenSort} \n\n");


                // === Direction Selection ===
                Console.WriteLine($"Direction: (Ascending, Descending) ");
                Console.Write("\n : ");
                directionInput = Console.ReadLine();

                // As there are only two directions, only one needs to be checked,
                // and the other can be 'defaulted'.
                if (directionInput.ToLower() == "descending")
                {
                    chosenDirection = Directions.Descending;
                }
                Console.WriteLine($"Direction Chosen: {chosenDirection} \n\n");

                // === Search Value Selection ===
                Console.WriteLine($"Value to search for:");
                Console.Write("\n : ");
                searchString = Console.ReadLine();
                searchValue = Int32.Parse(searchString);


                //state = (State)Enum.Parse(typeof(State), stateInput);
            }
            catch (FormatException) {
                Console.WriteLine("Invalid data for search-value");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            List<TimeSpan> times = new List<TimeSpan>();

            // Handles the calling and timing of the sort method
            DateTime startTime = DateTime.Now;

            file.Sort(chosenSort, chosenDirection);

            DateTime endTime = DateTime.Now;
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // Handles searching for the specified value
            startTime = DateTime.Now;

            (int Number, int[] indexes) searchResults = file.BinarySearch(0, file.getCount() - 1, searchValue, (int)chosenDirection);

            endTime = DateTime.Now;
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // Handles getting the data-intervals
            startTime = DateTime.Now;

            List<int> intervals = file.FindIntervals();

            endTime = DateTime.Now;
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // Logs all of the analysis processes to the console.
            Output logger = new Output();
            logger.WriteSortResults(file, times.ElementAt(0));
            logger.WriteSearchResults(searchResults, times.ElementAt(1));
            logger.WriteIntervals(intervals);

        }

        public static DataCollection MergeFiles(DataCollection leftFile, DataCollection rightFile) {

            // Sort the two files before merging.
            leftFile.Sort(SortTypes.MergeSort, Directions.Ascending);
            rightFile.Sort(SortTypes.MergeSort, Directions.Ascending);

            DataCollection mergedFile = new DataCollection("mergedFile");
            List<int> mergedData = mergedFile.MergeLists(leftFile.getData(), rightFile.getData(), (int) Directions.Ascending);
            mergedFile.setData(mergedData);
            return mergedFile;
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