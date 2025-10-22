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
    public class CustomFormatReaderTests
    {
        [Fact]
        public void Read_ShouldReturnCustomFormat()
        {
            //Arrange
            string input = "name:John age:30 score:92";

            PersonGame expected = new PersonGame("John", 30, 92);
            
            //Act
            CustomFormatReader reader = new CustomFormatReader(new StringReader(input));
            Result<PersonGame> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Data!);
        }        

        [Theory]
        [InlineData("name:John age:30 score:92")]
        [InlineData("name:John score:92 age:30")]
        [InlineData("score:92 name:John age:30")]
        [InlineData("age:30 name:John score:92")]
        [InlineData("age:30 score:92 name:John")]
        [InlineData("score:92 age:30 name:John")]
        public void Read_ShouldReturnEqualResults(string input)
        {   
            //Arrange
            PersonGame expected = new PersonGame("John", 30, 92);

            //Act
            CustomFormatReader reader = new CustomFormatReader(new StringReader(input));
            Result<PersonGame> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Data!);

        }

        [Theory]
        [InlineData("name:John age:30")]
        [InlineData("name:John age:30 score:92 weight:32")]
        [InlineData("name!:John age:30 score:92")]
        [InlineData("name:Ana age:31.21 score:34")]
        [InlineData("Alice 2 2")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new CustomFormatReader(new StringReader(input));
            Result<PersonGame> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
