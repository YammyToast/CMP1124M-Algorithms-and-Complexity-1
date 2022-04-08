using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMP1124M_Algorithms_and_Complexity_1
{
    internal class Output
    {
        /// <summary>
        /// Writes the sort results into the console in a table format.
        /// </summary>
        /// <param name="collection"> The data collection that performed the sort </param>
        /// <param name="ticksTaken"> The time-span over which the sort was performed.</param>
        public void WriteSortResults(DataCollection collection, TimeSpan ticksTaken) {
            // Holds the rows of the table to be iterated over.
            string[,] lines = new string[4, 2]{
                { $"Filename  " , $"{collection.fileName}" },
                { $"Data Count" , $"{collection.getCount()}" },
                { $"Direction ", $"{(Directions) collection.sortDirection}" },
                { $"Time Taken", $"{ticksTaken.TotalMilliseconds}ms" }
            };
            // Holds:
            // 1. The longest total length of a line.
            // 2. The longest data value for each category.
            int[] longestLengths = { 0, 0 };
            // Iterates through the rows and finds each of the longest values.
            for (int lengthIndex = 0; lengthIndex < lines.GetLength(0); lengthIndex++) {
                if (longestLengths[0] < (lines[lengthIndex, 0].Length + lines[lengthIndex, 1].Length)) { 
                    longestLengths[0] = lines[lengthIndex, 0].Length + lines[lengthIndex, 1].Length;
                }
                if (longestLengths[1] < lines[lengthIndex, 1].Length) {
                    longestLengths[1] = lines[lengthIndex, 1].Length;
                }
            }

            // Establishes the necessary amount of spaces and bars needed to arrange the table.
            string bars = new string('─', longestLengths[0]);
            string spaces = new string(' ', longestLengths[1]);

            // Writes the actual table.
            // Writes the header.
            Console.WriteLine($"┌{bars[..(bars.Length / 2 - 1)]} Stats {bars[..(bars.Length - (bars.Length / 2) - 1)]}┐");
            // Iterates over the predetermined rows of the table.
            // ! Spaces the second value using the established spaces to make sure they are all lined up. 
            for (int i = 0; i < lines.GetLength(0); i++) {
                Console.WriteLine($"│ {lines[i, 0]} : {lines[i, 1]}{spaces[..(longestLengths[1] - lines[i, 1].Length)]} │");
            }
            // Writes the footer of the table.
            // This could be changed as it is part static, and part dynamic.
            Console.WriteLine($"└{bars[..(bars.Length / 2 - 1)]}───────{bars[..(bars.Length - (bars.Length / 2) - 1)]}┘");
        }

        public void WriteIntervals(List<int> intervals, int intervalSpacing, TimeSpan ticksTaken) {
            // yeah
            int[] longestLengths = { intervals.Count().ToString().Length , (intervals.First() > intervals.Last()) ? intervals.First().ToString().Length : intervals.Last().ToString().Length };
            string bars = new string('─', longestLengths[0] + longestLengths[1]);
            string spaces = new string(' ', (longestLengths[0] > longestLengths[1]) ? longestLengths[0]:longestLengths[1]);
            int index = 1;

            Console.WriteLine($"┌{bars[..(bars.Length / 2 - 1)]} Intervals {bars[..(bars.Length - (bars.Length / 2) - 1)]}┐");
            

            foreach (int interval in intervals) {
                Console.WriteLine($"│ {spaces[..(longestLengths[0] - index.ToString().Length)]}{index}   :   {interval}{spaces[..(longestLengths[1] - interval.ToString().Length)]} │");
                index++;
            }

            Console.WriteLine($"└{bars[..(bars.Length / 2 - 1)]}───────────{bars[..(bars.Length - (bars.Length / 2) - 1)]}┘");
            Console.WriteLine($" Time: {ticksTaken.TotalMilliseconds}ms");
        }
    }
}
