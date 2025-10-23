using InputReaderApp.Readers;
using InputReaderApp.Tests.Helpers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers.ExpressionTree
{
    public class ExpressionReaderTests
    {
        [Fact]
        public void Read_ShouldReturnDouble()
        {
            //Arrange
            string input = "3 + 5 * (2 - 1)";

            Double expected = 8;            

            //Act
            ExpressionReader reader = new ExpressionReader(new StringReader(input));
            Result<Double> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess, result.Message);//succeed 
            Assert.Equal(expected, result.Data!);

        }
        [Theory]
        [InlineData("(3+5*(2-1)/2)", 5.5)]
        [InlineData("3+5*(2-1)", 8)]
        [InlineData("3 + 5 * 2 - 1", 12)]
        [InlineData("3 + 5 * 2 * 4 / 2 - 1", 22)]
        [InlineData("((3 + 5 * 2) * 4 )/ 2 - 1", 25)]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input, double expected)
        {
            //Arrange
            var reader = new ExpressionReader(new StringReader(input));

            // Act
            Result<double> result = reader.Read();

            // Assert
            Assert.True(result.IsSuccess, result.Message);
            Assert.Equal(expected, result.Data);
        }

        [Theory]
        [InlineData("3 + 5 * (2 - 1))")]
        [InlineData("(3 + () 5 * (2 - 1))")]
        [InlineData("3 + 5 *  * (2 - 1)")]
        [InlineData("3 + 5a * (2 - 1)")]
        [InlineData(" + 3 + 5 * (2 - 1)")]
        [InlineData("3 + 5 * (2( - 1))")]
        [InlineData("3 + 5 * (2 2) + 1")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new ExpressionReader(new StringReader(input));
            Result<Double> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
