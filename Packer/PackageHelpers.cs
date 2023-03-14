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
    public static class PackageHelpers
    {

        public static List<string> ValidateFile(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new APIException($"File '{filePath}' requested does not exist. Please check filename and location");
            }
            List<string> lines = File.ReadAllLines(filePath).ToList();

            if(lines.Count == 0)
            {
                throw new APIException($"File '{filePath}' was read succesfully, but found no lines of data. Please check file has data in correct format.");
            }

            return lines;
        }

        public static (int, List<PackageItem>) ValidateInput(string line)
        {
            string[] data = line.Split(':');
            if (int.TryParse(data[0].Trim(), out int weightLimit) == false)
            {
                throw new APIException($"Unable able to determine the weight of package. 'Received {data[0].Trim()}' expected '##' that is not higher than {PackageConstants.MAX_WEIGHT_OF_PACKAGE}");
            }
            else if (weightLimit > PackageConstants.MAX_WEIGHT_OF_PACKAGE)
            {
                throw new APIException($"The requested package weight of '{weightLimit}' exceeds the maximum allowed cost of {PackageConstants.MAX_WEIGHT_OF_PACKAGE}");
            }


            List<string> initialItems = new List<string>();
            List<PackageItem> items = new List<PackageItem>();

            foreach (string item in data[1].Split(' '))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                else if (initialItems.Count > PackageConstants.MAX_ITEMS)
                {
                    throw new APIException($"Total items exceced max allowed items of {PackageConstants.MAX_ITEMS}");
                }
                else
                {
                    initialItems.Add(item);
                }
            }

            foreach (var item in initialItems)
            {
                string[] itemData = item.Trim('(', ')').Split(',');
                int index = int.Parse(itemData[0]);
                if (decimal.TryParse(itemData[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal weight) == false)
                {
                    throw new APIException($"Unable able to determine the weight of item '{index}'. 'Received {itemData[1]}' expected '##.##'");
                }
                else if (weight > PackageConstants.MAX_WEIGHT_OR_COST_ITEM)
                {
                    throw new APIException($"Item '{index}' that weights {weight} exceeds the maximum allowed weight of {PackageConstants.MAX_WEIGHT_OR_COST_ITEM}");
                }

                if (decimal.TryParse(itemData[2].TrimStart('€'), out decimal cost) == false)
                {
                    throw new APIException($"Unable able to determine the cost of item '{index}'. 'Received {itemData[2]}' expected '€##'");
                }
                else if (weight > PackageConstants.MAX_WEIGHT_OR_COST_ITEM)
                {
                    throw new APIException($"Item '{index}' that costs {cost} exceeds the maximum allowed cost of {PackageConstants.MAX_WEIGHT_OR_COST_ITEM}");
                }
                items.Add(new PackageItem(index, weight, cost));
            }

            return (weightLimit, items);
        }

        public static List<List<PackageItem>> CreateCombinations(int weightLimit, List<PackageItem> items)
        {
            int maxWeight = weightLimit;
            List<List<PackageItem>> itemDictionary = new List<List<PackageItem>>();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Weight > maxWeight)
                {
                    continue;
                }
                var item = items[i];
                var listSize = itemDictionary.Count;

                for (int z = 0; z < listSize; z++)
                {
                    List<PackageItem> combo = itemDictionary[z];
                    List<PackageItem> newCombo = new List<PackageItem>(combo)
                    {
                        item
                    };
                    itemDictionary.Add(newCombo);
                }
                List<PackageItem> currentCombo = new List<PackageItem>
                {
                    item
                };
                itemDictionary.Add(currentCombo);

            }

            return itemDictionary;

        }

        public static List<PackageItem> FindBestCombination(List<List<PackageItem>> combinations, decimal weightlimit)
        {
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
                        bestCost = totalCost;
                        bestWeight = totalWeight;
                        bestCombination = combination;
                    }
                    else if (totalCost == bestCost && totalWeight < bestWeight)
                    {
                        bestCost = totalCost;
                        bestWeight = totalWeight;
                        bestCombination = combination;
                    }
                }
            }

            return bestCombination;
        }

        private static decimal GetCost(List<PackageItem> items)
        {
            decimal totalCost = 0;
            items.ForEach(x => totalCost += x.Cost);
            return totalCost;
        }

        private static decimal GetWeight(List<PackageItem> items)
        {
            decimal totalWeight = 0;
            items.ForEach(x => totalWeight += x.Weight);
            return totalWeight;
        }


    }
}
