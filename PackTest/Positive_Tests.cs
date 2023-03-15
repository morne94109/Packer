using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.IO;
using com.mobiquity.packer.Packer;

namespace Packer_Test
{
    public class Positive_Tests
    {
        public static IEnumerable<object[]> CC_Data =>
           new List<object[]>
           {
                new object[]
                    { "81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)", "31" },
                new object[] { "8 : (1,15.3,€34) ", "0" },
                new object[]
                {
                    "75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)",
                    "31"
                },
                new object[]
                {
                    "56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)",
                    "127"
                },
                new object[]
                {
                    "50 : (1,10,€20) (2,20,€30) (3,30,€40) (4,40,€50)",
                    "15"
                },
                new object[]
                    { "100 : (1,60,€70) (2,20,€20) (3,40,€80) (4,80,€100) (5,50,€50) (6,10,€10) (7,30,€30)",
                        "127" },
                new object[]
                {
                    "70 : (1,10,€20) (2,20,€30) (3,30,€40) (4,40,€50) (5,10,€15)",
                    "31"
                },
                new object[]
                {
                    "15 : (1,5,€10) (2,10,€20) (3,15,€30)",
                    "7"
                },
                new object[]
                {
                    "45 : (1,30,€20) (2,10,€10) (3,20,€40) (4,25,€35)",
                    "15"
                },
                 new object[]
                {
                   "100 : (1,5,€10) (2,10,€20) (3,15,€30) (4,20,€40) (5,25,€50) (6,30,€60) (7,35,€70) (8,40,€80) (9,45,€90) (10,50,€100) (11,55,€90) (12,60,€20) (13,65,€30) (14,70,€40) (15,75,€50)",
                    "32767"
                }               
           };

        public static IEnumerable<object[]> FBC_Data =>
            new List<object[]>
            {
                new object[]
                {
                    "81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(4, (decimal)72.30, 76)
                        }
                    }
                },
                new object[]
                {
                    "8 : (1,15.3,€34) ",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                        },
                    }
                },
                new object[]
                {
                    "75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(2, (decimal)14.55, 74),
                            new PackageItem(7, (decimal)60.02, 74)
                        }
                    }
                },
                new object[]
                {
                    "56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(8, (decimal)19.36, 79),
                            new PackageItem(9, (decimal)6.76, 64)
                        }
                    }
                },
                new object[]
                {
                    "50 : (1,10,€20) (2,20,€30) (3,30,€40) (4,40,€50)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(2, (decimal)20, 30),
                            new PackageItem(3, (decimal)30, 40)
                        },
                    }
                },
                new object[]
                { "100 : (1,60,€100) (2,20,€20) (3,40,€80) (4,38,€80) (5,50,€50) (6,10,€10) (7,30,€30)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(1, (decimal)60, 70),
                            new PackageItem(4, (decimal)39, 80)

                        },
                    } },
                new object[]
                {
                    "70 : (1,10,€20) (2,20,€30) (3,30,€40) (4,40,€50) (5,10,€15)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(1, (decimal)10, 20),
                            new PackageItem(2, (decimal)20, 30),
                            new PackageItem(3, (decimal)30, 40),
                            new PackageItem(5, (decimal)10, 15)
                        },
                    }
                },
                new object[]
                {
                    "15 : (1,5,€10) (2,10,€20) (3,15,€30)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(1, (decimal)5, 10),
                            new PackageItem(2, (decimal)10, 20)
                        },
                    }
                },
                new object[]
                {
                    "45 : (1,30,€20) (2,15,€40) (3,20,€40) (4,25,€35)",
                    new List<List<PackageItem>>()
                    {
                        new List<PackageItem>()
                        {
                            new PackageItem(2, (decimal)15, 40),
                            new PackageItem(3, (decimal)20, 40)
                        },
                    }
                }
            };

        public static IEnumerable<object[]> TVI_Data =>
        new List<object[]>
        {
                    new object[]
                    {
                        "81 : (1,53.38,€45) (2,88.62,€98)(3,78.48,€3)",
                       (81,
                            new List<PackageItem>()
                            {
                                new PackageItem(1, (decimal)53.38, 45),
                                new PackageItem(2, (decimal)88.62, 98),
                                new PackageItem(3, (decimal)78.48, 3)
                            }
                            )

                    },

        };

        [Theory]
        [InlineData("81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)", "PackFile.txt", "4")]
        [InlineData("8 : (1,15.3,€34)", "PackFile.txt", "-")]
        [InlineData("75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6, 76.25,€75) (7, 60.02,€74)(8, 93.18,€35)(9, 89.95,€78)", "PackFile.txt", "2,7")]
        [InlineData("56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6, 48.77,€79) (7, 81.80,€45)(8, 19.36,€79)(9, 6.76,€64)", "PackFile.txt", "8,9")]
        public void Test_Pack(string input, string filePath, string expectedResult)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.WriteAllText(filePath, input);

            var result = Packer.Pack(filePath);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("testfile.txt", "This is a test")]
        public void Test_ValidateFile(string filepath, string expectedMessage)
        {

            File.WriteAllText(filepath, expectedMessage);

            var parsedData = PackageHelpers.ValidateFile(filepath);

            Assert.True(parsedData[0].Equals(expectedMessage));
        }

        [Theory]
        [MemberData(nameof(TVI_Data))]
        public void Test_ValidateInput(string data, (int, List<PackageItem>) expectedMessage)
        {

            var parsedData = PackageHelpers.ValidateInput(data);
            var weight = parsedData.Item1;
            var listItems = parsedData.Item2;

            Assert.Equal((int)expectedMessage.Item1, parsedData.Item1);

            foreach (var item in expectedMessage.Item2)
            {
                var match = listItems.FirstOrDefault(x => x.Index == item.Index);

                Assert.True(match != null);
                Assert.True(match.Weight == item.Weight);
                Assert.True(match.Cost == item.Cost);

                listItems.Remove(match);
            }

            Assert.True(listItems.Count == 0);

            Console.WriteLine();
        }

        [Theory]
        [MemberData(nameof(CC_Data))]
        public void TestCombinationCreation(string data, string totalCombinations)
        {
            var parsedData = PackageHelpers.ValidateInput(data);

            var result = PackageHelpers.CreateCombinations(parsedData.Item1, parsedData.Item2);

            Assert.Equal(int.Parse(totalCombinations), result.Count);
        }

        [Theory]
        [MemberData(nameof(FBC_Data))]
        public void Test_FindBestCombination(string data, List<List<PackageItem>> bestCombination)
        {
            var parsedData = PackageHelpers.ValidateInput(data);

            var newData = PackageHelpers.CreateCombinations(parsedData.Item1, parsedData.Item2);

            var result = PackageHelpers.FindBestCombination(newData, parsedData.Item1);

            foreach (var item in result)
            {
                Assert.True(bestCombination.Exists(x => x.Exists(s => s.Index == item.Index)));
            }
        }

    }
}
