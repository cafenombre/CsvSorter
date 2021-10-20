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
            //Encoding.GetEncoding("iso-8859-1");
            //List all Csv files in the input/ directory
            List<string> allCsv = Directory.EnumerateFiles("input/", "*.csv", SearchOption.AllDirectories).ToList();

            List<string> SelectedHeaders = new List<string>();
            /*SelectedHeaders.Add("Key");
            SelectedHeaders.Add("Title");*/

            List<int> SelectedIndexes = new List<int>();// new int[] { 1, 3, 4 });//, 5, 11, 17, 19, 20, 27, 28, 39, 42, 44, 45, 61 });

            //Headers informations
            string[] header = { File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l)) };
            string filetered = header.FirstOrDefault().Replace("\"", "ZBOUB");
            string[] headers = filetered.Split("ZBOUB,ZBOUB");

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
                string filterino = line.Replace("\"", "ZBOUB");
                string[] splitData = filterino.Split("ZBOUB,ZBOUB");
                string sortedLine = "";
                foreach(int index in SelectedIndexes)
                {
                    sortedLine += splitData[index] + ",";
                }
                finalTable.Add(sortedLine.Remove(sortedLine.Length - 1));
            }



            
            File.WriteAllLines("output/wz.csv", finalTable, Encoding.UTF8);
        }
    }
}
