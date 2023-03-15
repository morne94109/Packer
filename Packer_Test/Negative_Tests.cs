using com.mobiquity.packer.Helpers;
using com.mobiquity.packer.Exceptions;
using Xunit;
using System.IO;
using com.mobiquity.packer.Packer;

namespace Packer_Test
{
    public class Negative_Tests
    {
        [Theory]
        [InlineData("NegPackFile.txt",
            "An error occurred while processing the file.",
            "File 'NegPackFile.txt' requested does not exist. Please check filename and location")]
        public void Test_Negative_Pack(string filePath, string expectedMessage, string expectedInner)
        {
            var exception = Assert.Throws<APIException>(() =>
            {
                var result = Packer.Pack(filePath);
            });

            Assert.Equal(expectedMessage, exception.Message);
            Assert.Equal(expectedInner, exception.InnerException.Message);
        }

        [Theory]
        [InlineData("NegTestfile1.txt", "File 'NegTestfile1.txt' requested does not exist. Please check filename and location")]
        [InlineData("NegTestfile2.txt", "File 'NegTestfile2.txt' was read successfully, but found no lines of data. Please check file has data in correct format.", true)]
        public void Test_Negative_ValidateFile(string filepath, string expectedMessage, bool setupFile = false)
        {
            if (setupFile)
            {
                File.WriteAllText(filepath, "");
            }
            var exception = Assert.Throws<APIException>(() =>
            {
                var parsedData = PackageHelpers.ValidateFile(filepath);
            });

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("1H : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9)",
            "Unable able to determine the weight of package. Received '1H' expected '##' that is not higher than 100")]
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
        [InlineData("80 : (1,5,€10) (2,10,€20) (3,15,€30) (4,20,€40) (5,25,€50) (6,30,€60) (7,35,€70) (8,40,€80) (9,45,€90) (10,50,€100) (11,55,€90) (12,60,€20) (13,65,€30) (14,70,€40) (15,75,€50) (16,80,€60)",
            "Total items exceeded max allowed items of 15")]
        public void Test_Negative_ValidateInput(string data, string expectedMessage)
        {
            var exception = Assert.Throws<APIException>(() =>
            {
                var parsedData = PackageHelpers.ValidateInput(data);
            });

            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
