using InputReaderApp.Readers;
using InputReaderApp.Tests.Helpers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers
{
    public class ArrayReaderTests
    {
        [Fact]
        public void Read_ShouldReturnArray_1()
        {
            //Arrange
            string input =  "3 4" + "\n" +
                            "1 2 3 4" + "\n" +
                            "5 6 7 8" + "\n" +
                            "9 10 11 12";                                                        

            int[,] expected = {
                    {1, 2, 3, 4},
                    {5, 6, 7, 8},
                    {9, 10, 11, 12}
                };

            //Act
            ArrayReader reader = new ArrayReader(new StringReader(input));
            Result<int[,]> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.Equal2D(expected, result.Data!);            

        }
        [Fact]
        public void Read_ShouldReturnArray_2()
        {
            //Arrange
            string input = "4 6" + "\n" +
                            "1 2 3      4 5  6" + "\n" +
                            "5 6 7 8 9 10" + "\n" +
                            "9 10 11 12         13  14" + "\n" +
                            "11 12 13 14         15  16";

            int[,] expected = {
                    {1, 2, 3, 4 , 5, 6},
                    {5, 6, 7, 8, 9 , 10},
                    {9, 10, 11, 12, 13, 14},
                    {11, 12, 13, 14, 15 , 16 },
                };
            //Act
            ArrayReader reader = new ArrayReader(new StringReader(input));
            Result<int[,]> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.Equal2D(expected, result.Data!);            

        }

        [Theory]
        [InlineData("two 3\n1 2 3\n4 5 6")]
        [InlineData("2 2\n1 x\n3 4")]
        [InlineData("3 2\n1 2\n3 4\n5 a")]
        [InlineData("3 2\n1 2\n3 4\n5 6.2")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new ArrayReader(new StringReader(input));
            Result<int[,]> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);            
        }

        [Theory]
        [InlineData("2 3\n1 2 3\n4 5")]        // Too few elements in the second row
        [InlineData("2 3\n1 2 3\n4 5 6 7")]    // Too many elements in the second row
        public void Read_ShouldFail_WhenRowHasIncorrectNumberOfElements(string input)
        {
            // Act
            var reader = new ArrayReader(new StringReader(input));    // Set up the object under test                                                                     // Act
            Result<int[,]> result = reader.Read();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
            
        }

        [Theory]
        [InlineData("3\n1 2 3")]          // Too few numbers on first line
        [InlineData("2 3 4\n1 2 3\n4 5 6")] // Too many numbers on first line
        public void Read_ShouldFail_WhenDimensionLineIsInvalid(string input)
        {
            //Act
            var reader = new ArrayReader(new StringReader(input));
            Result<int[,]> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
            
        }
    }
}
