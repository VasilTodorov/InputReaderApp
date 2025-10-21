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
    public class NestedListReaderTests
    {
        [Fact]
        public void Read_ShouldReturnNestedList_1()
        {
            //Arrange
            string input = "1 2" + "\n" +
                            "3 4 5" + "\n" +
                            "6 " + "\n" +
                            "7 8 9 10 11";

            List<List<int>> expected = new List<List<int>>();

            expected.Add(new List<int> { 1, 2});
            expected.Add(new List<int> { 3, 4, 5});
            expected.Add(new List<int> { 6});
            expected.Add(new List<int> { 7, 8, 9, 10, 11});

            //Act
            NestedListReader reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.EqualNestedLists(expected, result.Data!);
            
        }
        [Fact]
        public void Read_ShouldReturnNestedList_2()
        {
            //Arrange
            string input = "1 2 3 4 5 6" + "\n" +
                            "3 4 5 6 7 8" + "\n" +
                            "6 7 8 9 10 11" + "\n" +
                            "7 8 9 10 11 12";

            List<List<int>> expected = new List<List<int>>();

            expected.Add(new List<int> { 1, 2, 3, 4, 5, 6});
            expected.Add(new List<int> { 3, 4, 5, 6, 7, 8 });
            expected.Add(new List<int> { 6, 7, 8, 9, 10, 11 });
            expected.Add(new List<int> { 7, 8, 9, 10, 11, 12 });

            //Act
            NestedListReader reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.EqualNestedLists(expected, result.Data!);
        }

        [Theory]
        [InlineData("two 3\n1 2 3\n4 5 6")]
        [InlineData("2 2\n1 x\n3 4")]
        [InlineData("3 2\n1 2\n3 4\n5 a")]
        [InlineData("4 2\n1 2\n3 4\n5 6.2")]
        [InlineData("hello\n1 2\n3 4\n5 5")]
        [InlineData("1 2\n1 2\n3+4\n5 6")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);            
        }
    }
}
