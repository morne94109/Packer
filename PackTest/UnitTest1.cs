using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PackTest
{
    public class UnitTest1
    {
        public static IEnumerable<object[]> CC_Data =>
           new List<object[]>
           {
                new object[] { "81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)", "31" },
                new object[] { "8 : (1,15.3,€34) ", "0"},
                new object[] { "75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)" , "31"},
                new object[] { "56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)" , "127" },
           };

        public static IEnumerable<object[]> FBC_Data =>
            new List<object[]>
            {
                        new object[] { "81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)",
                            new List<List<PackageItem>>()
                            {
                                new List<PackageItem>()
                                {
                                  new PackageItem (4,(decimal)72.30,76)
                                }
                            }
                        },
                        new object[] { "8 : (1,15.3,€34) ",
                            new List<List<PackageItem>>()
                            {
                                new List<PackageItem>()
                                {
                                  
                                },
                            }
                        },
                        new object[] { "75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)" ,
                            new List<List<PackageItem>>()
                            {
                                new List<PackageItem>()
                                {
                                  new PackageItem (2,(decimal)14.55,74),
                                  new PackageItem (7,(decimal)60.02,74)
                                }
                            }
                        },
                        new object[] { "56 : (1,90.72,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)" ,
                            new List<List<PackageItem>>()
                            {
                                new List<PackageItem>()
                                {
                                  new PackageItem (8,(decimal)19.36,79),
                                   new PackageItem (9,(decimal)6.76,64)
                                }
                            }
                        },
            };
    

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