using com.mobiquity.packer.Exceptions;
using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace com.mobiquity.packer.Packer
{
    public class Packer
    {
        static void Main(string[] args)
        {
            string input = args[0];
            var message = Pack(input);
            Console.WriteLine(message);
        }

        public static string Pack(string filePath)
        {
            string result = "";

            try
            {
                List<string> lines = PackageHelpers.ValidateFile(filePath);

                foreach (string line in lines)
                {
                    (int weightLimit, List<PackageItem> items) = PackageHelpers.ValidateInput(line);
                    var combinations = PackageHelpers.CreateCombinations(weightLimit, items);
                    var bestCombination = PackageHelpers.FindBestCombination(combinations, weightLimit);

                    if (bestCombination.Count == 0)
                    {
                        result += "-" + Environment.NewLine;
                    }
                    else
                    {
                        result += string.Join(",", bestCombination.Select(x => x.Index)) + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new APIException("An error occurred while processing the file.", ex);
            }

            return result.Trim();
        }

        
    }

    
}