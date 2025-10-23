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
    public class CilToolReaderTests
    {
        [Fact]
        public void Read_ShouldReturnCommand()
        {
            //Arrange
            string input = "command -f test.csv -o /output --quiet";
            Command expected = new Command("test.csv", "/output", true);            

            //Act
            CilToolReader reader = new CilToolReader(new StringReader(input));
            Result<Command> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Data!);
        }

        [Theory]
        [InlineData("command -f test.csv -o /output")]
        [InlineData("command -o /output -f test.csv")]
        [InlineData("command --quiet -o /output -f test.csv")]
        [InlineData("command -f test.csv --quiet -o /output")]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Act
            var reader = new CilToolReader(new StringReader(input));
            Result<Command> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("command -f test.csv -f hello.txt -o /output --quiet")]
        [InlineData("command -f -f test.csv -o /output --quiet")]
        [InlineData("command -f test.csv -o /output -quiet")]
        [InlineData("command -f test.csv --quiet")]
        [InlineData("command -f test.csv --quiet -o")]
        [InlineData("-f test.csv -o /output")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new CilToolReader(new StringReader(input));
            Result<Command> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
