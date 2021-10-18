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

            //Headers informations
            string[] header = { File.ReadLines(allCsv.First()).First(l => !string.IsNullOrWhiteSpace(l)) };

            var mergedData = allCsv
                .SelectMany(csv => File.ReadLines(csv)
                    .SkipWhile(l => string.IsNullOrWhiteSpace(l)).Skip(1)); // skip header of each file

            List<string> sortedDatas = mergedData.ToList();

            File.WriteAllLines("output/suce.csv", header.Concat(mergedData));
        }
    }
}
