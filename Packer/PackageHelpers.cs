using com.mobiquity.packer.Exceptions;
using com.mobiquity.packer.Models;
using com.mobiquity.packer.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mobiquity.packer.Helpers
{
    /// <summary>
    /// The package helpers class
    /// </summary>
    public static class PackageHelpers
    {

        /// <summary>
        /// Validates the file using the specified file path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <exception cref="APIException">File '{filePath}' requested does not exist. Please check filename and location</exception>
        /// <exception cref="APIException">File '{filePath}' was read successfully, but found no lines of data. Please check file has data in correct format.</exception>
        /// <returns>The lines</returns>
        public static List<string> ValidateFile(string filePath)
        {
            //Check that file does exist
            if (File.Exists(filePath) == false)
            {
                throw new APIException($"File '{filePath}' requested does not exist. Please check filename and location");
            }

            //Read file
            List<string> lines = File.ReadAllLines(filePath).ToList();

            //Check that file does contain any input
            if (lines.Count == 0)
            {
                throw new APIException($"File '{filePath}' was read successfully, but found no lines of data. Please check file has data in correct format.");
            }

            return lines;
        }

        /// <summary>
        /// Validates the input using the specified line
        /// </summary>
        /// <param name="line">The line</param>
        /// <exception cref="APIException">Item '{index}' that costs {cost} exceeds the maximum allowed cost of {PackageConstants.MAX_WEIGHT_OR_COST_ITEM}</exception>
        /// <exception cref="APIException">Item '{index}' that weights {weight} exceeds the maximum allowed weight of {PackageConstants.MAX_WEIGHT_OR_COST_ITEM}</exception>
        /// <exception cref="APIException">The requested package weight of '{weightLimit}' exceeds the maximum allowed cost of {PackageConstants.MAX_WEIGHT_OF_PACKAGE}</exception>
        /// <exception cref="APIException">Total items exceeded max allowed items of {PackageConstants.MAX_ITEMS}</exception>
        /// <exception cref="APIException">Unable able to determine the cost of item '{index}'. 'Received {itemData[2]}' expected '€##'</exception>
        /// <exception cref="APIException">Unable able to determine the weight of item '{index}'. 'Received {itemData[1]}' expected '##.##'</exception>
        /// <exception cref="APIException">Unable able to determine the weight of package. 'Received {data[0].Trim()}' expected '##' that is not higher than {PackageConstants.MAX_WEIGHT_OF_PACKAGE}</exception>
        /// <returns>An int and list of package item</returns>
        public static (int, List<PackageItem>) ValidateInput(string line)
        {
            string[] data = line.Split(':');
            if (int.TryParse(data[0].Trim(), out int weightLimit) == false)
            {
                throw new APIException($"Unable able to determine the weight of package. Received '{data[0].Trim()}' expected '##' that is not higher than {PackageConstants.MAX_WEIGHT_OF_PACKAGE}");
            }
            else if (weightLimit > PackageConstants.MAX_WEIGHT_OF_PACKAGE)
            {
                throw new APIException($"The requested package weight of '{weightLimit}' exceeds the maximum allowed cost of {PackageConstants.MAX_WEIGHT_OF_PACKAGE}");
            }


            // List<string> initialItems = new List<string>();
            List<PackageItem> items = new List<PackageItem>();

            var itemSplit = data[1].RemoveWhitespace().Split(new[] { ")(" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //Ensure that total items doesn't exceed 15
            if (itemSplit.Count > PackageConstants.MAX_ITEMS)
            {
                throw new APIException($"Total items exceeded max allowed items of {PackageConstants.MAX_ITEMS}");
            }

            List<string> splicedString = new List<string>();

            foreach (var item in itemSplit)
            {
                splicedString.Add(item.Trim('(', ')'));
            }

            foreach (var item in splicedString)
            {
                string[] itemData = item.Split(',');
                int index = int.Parse(itemData[0]);

                //Ensure that weight can be parsed and check if it meets any contrainsts
                if (decimal.TryParse(itemData[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal weight) == false)
                {
                    throw new APIException($"Unable able to determine the weight of item '{index}'. Received '{itemData[1]}' expected '##.##'");
                }
                else if (weight > PackageConstants.MAX_WEIGHT_OR_COST_ITEM)
                {
                    throw new APIException($"Item '{index}' that weights {weight} exceeds the maximum allowed weight of {PackageConstants.MAX_WEIGHT_OR_COST_ITEM}");
                }

                //Ensure that cost can be parsed and check if it meets any contrainsts
                if (decimal.TryParse(itemData[2].TrimStart('€'), out decimal cost) == false)
                {
                    throw new APIException($"Unable able to determine the cost of item '{index}'. Received '{itemData[2]}' expected '€##'");
                }
                else if (cost > PackageConstants.MAX_WEIGHT_OR_COST_ITEM)
                {
                    throw new APIException($"Item '{index}' that costs €{cost} exceeds the maximum allowed cost of €{PackageConstants.MAX_WEIGHT_OR_COST_ITEM}");
                }

                //Succesfully passed validation. Add to items list
                items.Add(new PackageItem(index, weight, cost));
            }

            //Return package weight limit and all items that can possibly fit
            return (weightLimit, items);
        }

        /// <summary>
        /// Create 2D list of all possible combinations, excluding combinations that weighs more than the weightLimit
        /// </summary>
        /// <param name="weightLimit"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<List<PackageItem>> CreateCombinations(int weightLimit, List<PackageItem> items)
        {
            int maxWeight = weightLimit;
            //Create 2D list
            List<List<PackageItem>> multiList = new List<List<PackageItem>>();

            foreach (var package in items)
            {
                if (package.Weight > maxWeight)
                {
                    continue;
                }

                var listSize = multiList.Count;

                for (int z = 0; z < listSize; z++)
                {
                    //Use previous items in list to create new list and add the current item to the list
                    List<PackageItem> newCombo = new List<PackageItem>(multiList[z])
                    {
                        package
                    };
                    //Add list to 2D list
                    multiList.Add(newCombo);
                }
                
                List<PackageItem> currentCombo = new List<PackageItem>
                {
                    package
                };

                //Add current item to 2D list
                multiList.Add(currentCombo);
            }

            return multiList;

        }

        /// <summary>
        /// Finds the best combination using the specified combinations
        /// </summary>
        /// <param name="combinations">The combinations</param>
        /// <param name="weightlimit">The weightlimit</param>
        /// <returns>The best combination</returns>
        public static List<PackageItem> FindBestCombination(List<List<PackageItem>> combinations, decimal weightlimit)
        {
            //Create list to store the most suitable combinations
            List<PackageItem> bestCombination = new List<PackageItem>();
            decimal maxWeight = weightlimit;
            decimal bestCost = 0;
            decimal bestWeight = 0;

            foreach (var combination in combinations)
            {
                decimal totalWeight = GetWeight(combination);
                if (totalWeight > maxWeight)
                {
                    continue;
                }
                else
                {
                    decimal totalCost = GetCost(combination);
                    if (totalCost > bestCost)
                    {
                        //Store store best values if condition is met
                        bestCost = totalCost;
                        bestWeight = totalWeight;
                        bestCombination = combination;
                    }
                    // if cost is the same check if the package is lighter
                    else if (totalCost == bestCost && totalWeight < bestWeight)
                    {
                        //Store store best values if condition is met
                        bestCost = totalCost;
                        bestWeight = totalWeight;
                        bestCombination = combination;
                    }
                }
            }

            return bestCombination;
        }

        /// <summary>
        /// Gets the cost using the specified items
        /// </summary>
        /// <param name="items"The list of items to calculate</param>
        /// <returns>The total cost</returns>
        private static decimal GetCost(List<PackageItem> items)
        {
            decimal totalCost = 0;
            items.ForEach(x =>
            {
                totalCost += x.Cost;
            });
            return totalCost;
        }

        /// <summary>
        /// Gets the weight using the specified items
        /// </summary>
        /// <param name="items">The list of items to calculate</param>
        /// <returns>The total weight</returns>
        private static decimal GetWeight(List<PackageItem> items)
        {
            decimal totalWeight = 0;
            items.ForEach(x =>
            {
                totalWeight += x.Weight;
            });
            return totalWeight;
        }

        /// <summary>
        /// Returns a string with all whitespaces removed
        /// </summary>
        /// <param name="input">The string input</param>
        /// <returns>String with no whitespaces</returns>
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

    }
}
