using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using com.mobiquity.packer.Exceptions;
using Xunit;

namespace PackTest
{
    public class UnitTest1
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

        [Theory]
        [InlineData("testfile.txt", "File 'testfile.txt' requested does not exist. Please check filename and location")]
        public void Test_Negative_ValidateFile(string filepath, string expectedMessage)
        {
            var exception = Assert.Throws<APIException>(() =>
            {
                var parsedData = PackageHelpers.ValidateFile(filepath);
            });
            
            Assert.Equal(expectedMessage,exception.Message);
        }
        
        [Theory]
        [InlineData("150 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9)", 
            "The requested package weight of '150' exceeds the maximum allowed cost of 100")]
        [InlineData("70 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,105.0,€29)", 
            "Item '5' that weights 105,0 exceeds the maximum allowed weight of 100")]
        [InlineData("90 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,$9)", 
            "Unable able to determine the cost of item '5'. Received '$9' expected '€##'")]
        [InlineData("70 : (1,53.38,€45) (2,88.62,€108) (3,78.48,€3) (4,72.30,€76) (5,105.0,€29)", 
            "Item '2' that costs €108 exceeds the maximum allowed cost of €100")]
        [InlineData("50 : (1,53.38,€45) (2,88.62,€98) (3,78-48,€3) (4,72.30,€76) (5,55.23,€25)", 
            "Unable able to determine the weight of item '3'. Received '78-48' expected '##.##'")]
        public void Test_Negative_ValidateInput(string data, string expectedMessage)
        {
            var exception = Assert.Throws<APIException>(() =>
            {
                var parsedData = PackageHelpers.ValidateInput(data);
            });
            
            Assert.Equal(expectedMessage,exception.Message);
        }
        
    }
}