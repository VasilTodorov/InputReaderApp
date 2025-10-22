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
        public void Read_ShouldReturnNestedList()
        {
            //Arrange
            string input = "[[1,2],[3,4,5],[6]]";

            List<List<int>> expected = new List<List<int>>();

            expected.Add(new List<int> {1,2});
            expected.Add(new List<int>() { 3, 4, 5});
            expected.Add(new List<int>() { 6});

            //Act
            NestedListReader reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess, result.Message);//succeed 
            AssertExtensions.EqualLists(expected, result.Data!);

        }
        [Theory]
        [InlineData("[[],[],[]]")]
        [InlineData("[[],[1],[2],[12]]")]
        [InlineData("[[0],[1 , 2],[     4,  32]]")]
        [InlineData("[]")]        
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Act
            var reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("[[1,2],[3,4,5],[6]")]
        [InlineData("[[1,2],[3,4,a],[6]]")]
        [InlineData("[[1,2],3,4,5],[6]]")]
        [InlineData("[[1,2],[[3,4,5],[6]]")]
        [InlineData("[[1,2],[[3,4,5]],[6]]")]
        [InlineData("[[1,2],8,[3,4,a],[6]]")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new NestedListReader(new StringReader(input));
            Result<List<List<int>>> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}

