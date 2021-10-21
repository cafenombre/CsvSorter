using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            //List all Csv files in the input/ directory
            List<string> allCsv = Directory.EnumerateFiles("input/", "*.csv", SearchOption.AllDirectories).ToList();

            List<string> SelectedHeaders = new List<string>();
            /*SelectedHeaders.Add("Key");
            SelectedHeaders.Add("Title");*/

            List<int> SelectedIndexes = new List<int>(new int[] {0, 2, 3, 4, 10, 16, 18, 19, 26, 27, 38, 41, 43, 44, 60 });

            //Headers informations
            string[] header = { File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l)) };
            string[] headers = header.FirstOrDefault().Split("\",\"");

            //Remove first "
            headers[0] = headers[0].Remove(0, 1);


            int i = 0;
            foreach(string h in headers)
            {
                int index = i;
                bool workingheader = SelectedHeaders.Where(u => h.ToLower().Trim() == "\"" + u.ToLower().Trim() + "\"").Any() ;
                
                if(workingheader)
                {
                    SelectedIndexes.Add(i);
                }

                i++;
            }

            var mergedData = allCsv
                .SelectMany(csv => File.ReadLines(csv)
                    .SkipWhile(l => string.IsNullOrWhiteSpace(l)).Skip(1)); // skip header of each file

            List<string> sortedDatas = mergedData.ToList();

            List<string> finalTable = new List<string>();

            string sortedHeader = "";
            foreach (int index in SelectedIndexes)
            {
                sortedHeader += headers[index] + ",";
            }
            finalTable.Add(sortedHeader);

            foreach (string line in sortedDatas)
            {
                //remove the first " before the split
                string lineS = line.Remove(0, 1);

                string[] splitData = lineS.Split("\",\"");
                string sortedLine = "";
                foreach(int index in SelectedIndexes)
                {
                    sortedLine += "\""+ splitData[index] + "\",";
                }
                finalTable.Add(sortedLine.Remove(sortedLine.Length - 1));
            }



            
            File.WriteAllLines("output/wz.csv", finalTable, Encoding.UTF8);
        }
    }
}
