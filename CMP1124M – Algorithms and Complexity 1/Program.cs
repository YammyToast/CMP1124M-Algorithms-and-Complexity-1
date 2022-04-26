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
                "Share_3_256", "Share_3_2048",
                "smalltest"
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
                // If an error occurs in the file loading, throw an error for the user.
                if (dataObjects.Count != dataFileNames.Count) {
                    throw new Exception($"{dataObjects.Count}/{dataFileNames.Count}");
                }

            }
            // If a critical error occurs during file loading, throw an exception.
            catch (Exception ex) {
                Console.WriteLine($"Could not create all data collections: {ex}");
            }

            // Test handler functions.
            // Uncomment to run tests.
            TestHandler handler = new TestHandler(dataObjects);
            //handler.RunTests();

            //handler.RunSearch();



            State state = State.UserInput;
            string stateInput = " ";

            // ======================== Main State Loop ========================

            while (state == State.UserInput) {
                Console.WriteLine($"1. Analyse a file | 2. Merge a file | 3. Exit ");
                try
                {
                    // Read-in user input, and attempt parse into valid enum.
                    Console.Write(" : ");
                    stateInput = Console.ReadLine();
                    state = (State)Enum.Parse(typeof(State), stateInput);


                    // ======================== Analyse Selected ========================
                    if (state == State.AnalyseFile)
                    {
                        // Display all of the loaded files NAMES to the user.
                        Console.WriteLine($"| Loaded Files |");
                        foreach (string file in dataFileNames)
                        {
                            Console.WriteLine($" > {file}");
                        }
                        Console.WriteLine($"Please Choose a file");
                        try
                        {
                            // Take in name input.
                            Console.Write($" : ");
                            string fileInput = Console.ReadLine();

                            // Check whether the given file name exists within the loaded names.
                            if (dataFileNames.Exists(x => x == fileInput))
                            {
                                // If file with given name exists, save the corresponding OBJECT.
                                DataCollection file = dataObjects.ElementAt(dataFileNames.IndexOf(fileInput));
                                // Call the analyseFile procedure with the file object.
                                AnalyseFile(file);
                            }
                            // If given file name doesn't exist, throw an exception.
                            else
                            {
                                throw new Exception("Invalid File Name");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex}");
                        }
                        // Return to stateInput whether successful or not.
                        state = State.UserInput;

        
                    }
                    // ======================== Merge Selected ========================
                    else if (state == State.MergeFile)
                    {
                        // Initialise two strings for the filenames.
                        string leftFileName = string.Empty;
                        string rightFileName = string.Empty;
                        try
                        {
                            // While no valid file-name has been given, continue to loop.
                            while (!dataFileNames.Exists(x => x == leftFileName)) {
                                // Display all of the file names to the user.
                                Console.WriteLine($"| Loaded Files |");
                                foreach (string file in dataFileNames)
                                {
                                    Console.WriteLine($" > {file}");
                                }
                                // Read in the user's input.
                                Console.WriteLine($"Choose the first file in the merge: \n\n");
                                Console.Write(" : ");
                                leftFileName = Console.ReadLine();
                            }
                            // Repeat loop state, but for second file name.
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
                        // Initialise a new Datacollection:
                        // Call the MergeFiles function, a helper function, passing in
                        // the selected files. 
                        DataCollection mergedFile = MergeFiles(dataObjects.ElementAt(dataFileNames.IndexOf(leftFileName)),
                                                                dataObjects.ElementAt(dataFileNames.IndexOf(rightFileName)));
                        // Add the new DataCollection to the list of dataObjects.
                        dataObjects.Add(mergedFile);
                        // Add a suffix if multiple mergedFiles have been created.
                        string fileSuffix = (mergedFileCounter > 0) ? $"({mergedFileCounter})" : "";
                        // Add the fileName to the list of names.
                        dataFileNames.Add($"{mergedFile.fileName}{fileSuffix}");
                        // Increment the suffix counter, and then return to state selection.
                        mergedFileCounter++;
                        state = State.UserInput;

                    }
                    // ======================== Exit Selected ========================
                    else if (state == State.Exit) 
                    { 
                        Environment.Exit(0);
                    }

                    // ======================== Invalid State ========================
                    else
                    {
                        // If a state could not be parsed from the input, repeat until a valid input is given.
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
            
        }

        /// <summary>
        /// Runs the Anaylse file procedure on the given file.
        /// </summary>
        /// <param name="file">Datacollection to be analysed.</param>
        public static void AnalyseFile(DataCollection file) {

            // Initialise and set default values.
            SortTypes chosenSort  = SortTypes.Merge;
            SearchTypes chosenSearch = SearchTypes.Binary;
            Directions chosenDirection = Directions.Ascending;
            string sortInput = string.Empty;
            string directionInput = string.Empty;
            string searchInput = string.Empty;
            string searchString = string.Empty;
            int searchValue = 0;

            
            Console.WriteLine($"\n\n Analyse | {file.fileName} | {file.getCount()} \n\nAvailable Sorts: (Defaults: {chosenSort}, {chosenDirection})");
            // Inputting is wrapped in a try clause to handle null-arguments.
            try
            {
                // ======================== Sort Selection ========================
                foreach (SortTypes sort in Enum.GetValues(typeof(SortTypes)))
                {
                    Console.Write($"| {sort} |");
                }
                Console.Write("\n : ");
                sortInput = Console.ReadLine();
                // Attempt to parse given input to a valid SortType.

                // ! If no valid sort can be parsed, a default value has already been initialised.

                foreach (SortTypes checkSort in Enum.GetValues(typeof(SortTypes)))
                {
                    if (sortInput.ToLower() == checkSort.ToString().ToLower())
                    {
                        // If a valid sort is given, overwrite the default.
                        chosenSort = checkSort;
                    }
                }
                // ! Log the selected sort back to the user.
                Console.WriteLine($"Using Sort: {chosenSort} \n\n");


                // ======================== Direction Selection ========================
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

                // ======================== Search Value Selection ========================
                Console.WriteLine($"Value to search for:");
                Console.Write("\n : ");
                searchString = Console.ReadLine();
                // Attempt to parse selected search value into an int.
                // If attempt fails, a FormatException is thrown.
                searchValue = Int32.Parse(searchString);
                Console.WriteLine($"Search Value Selected: {searchValue} \n\n");

                // ======================== Search Algorithm Selection ========================
                if (searchString != string.Empty) {
                    Console.WriteLine($"Search Algorithm to use: (Binary, Jump)  (Default: Binary)");
                    Console.Write("\n : ");
                    searchInput = Console.ReadLine();
                    foreach (SearchTypes checkSearch in Enum.GetValues(typeof(SearchTypes)))
                    {
                        if (searchInput.ToLower() == checkSearch.ToString().ToLower())
                        {
                            chosenSearch = checkSearch;
                        }
                    }
                    Console.WriteLine($"Searching using {chosenSearch}");
                }

            }
            catch (FormatException) {
                Console.WriteLine("Invalid data for search-value");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // ======================== Procedure Calls and Time Recording ========================
            List<TimeSpan> times = new List<TimeSpan>();

            // ============= Sort =============
            // Saves the time at which the start of the sort occured.
            DateTime startTime = DateTime.Now;
            
            // Call the sort handler method to perform the desired sort.
            file.Sort(chosenSort, chosenDirection);

            // Save the end time after the sort has been performed.
            DateTime endTime = DateTime.Now;
            // Save the time that it took to the list of times.
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // ============= Value Search =============
            startTime = DateTime.Now;

            (int Number, int[] indexes) searchResults = file.Search(chosenSearch, searchValue, (int) chosenDirection);

            endTime = DateTime.Now;
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // ============= Intervals =============
            startTime = DateTime.Now;

            List<int> intervals = file.FindIntervals();

            endTime = DateTime.Now;
            times.Add(TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks));

            // ======================== Output Calls ========================
            Output logger = new Output();
            logger.WriteSortResults(file, times.ElementAt(0));
            logger.WriteSearchResults(searchResults, times.ElementAt(1), file.steps);
            logger.WriteIntervals(intervals);

        }

        /// <summary>
        /// Merge Files 'pre' function.
        /// </summary>
        /// <param name="leftFile">First file in the merge</param>
        /// <param name="rightFile">Second file in the merge</param>
        /// <returns></returns>
        public static DataCollection MergeFiles(DataCollection leftFile, DataCollection rightFile) {

            // Sort the two files before merging.
            leftFile.Sort(SortTypes.Merge, Directions.Ascending);
            rightFile.Sort(SortTypes.Merge, Directions.Ascending);

            // Initialise a new dataCollection.
            DataCollection mergedFile = new DataCollection("mergedFile");
            // Call the mergeLists function, merging the two given lists.
            // Save the mergedList in a temporary variable.
            List<int> mergedData = mergedFile.MergeLists(leftFile.getData(), rightFile.getData(), (int) Directions.Ascending);
            // Set the data of the new dataCollection to the merged list.
            mergedFile.setData(mergedData);
            // Return the completed dataCollection.
            return mergedFile;
        }

        /// <summary>
        /// Reads a file into the program file-storage.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<int> ReadData(string fileName) {
            // Initialise a new list to store the file data in.
            List<int> dataList = new List<int>();
            int lineNumber = 1;

            // Try to open the file with the given file name.
            try
            {
                // Parse and locate the working directory.
                string localFileDir = Environment.CurrentDirectory;
                string[] subs = localFileDir.Split(@"\bin");
                // Concat the filename with the needed file-types.
                string fileDir = Path.Combine(subs[0], @"data\" + fileName + @".txt");
                // Read all of the data in the file into an array, indexing it based on new-lines.
                string[] data = File.ReadAllText(fileDir).Split("\n");
                
                // As data is read in as a string, it needs to be parsed to integers.
                // For each line that was read in, check that it can be parsed to an int.
                foreach (string s in data) {
                    int num;
                    if (int.TryParse(s, out num))
                    {
                        // If can be parsed as an int, add to list of data-values.
                        dataList.Add(num);
                        // Increment line number.
                        lineNumber++;
                    }
                    else {
                        // If a value that can't be parsed as an int is read, an exception is thrown.
                        throw new ArrayTypeMismatchException(s);
                    }
                }
                
            }
            // Reporting of any exceptions to the user.
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