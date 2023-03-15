using com.mobiquity.packer.Exceptions;
using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Packer_Test")]
namespace com.mobiquity.packer.Packer
{
    /// <summary>
    /// The packer class
    /// </summary>
    public class Packer
    {       
        /// <summary>
        /// Packs the items found in the file from file path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <exception cref="APIException">An error occurred while processing the file. </exception>
        /// <returns>The string</returns>
        internal static string Pack(string filePath)
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