using InputReaderApp.Readers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers.DifferentStates
{
    public class ReadLocationTests
    {
        [Fact]
        public void Read_ShouldReturnLocation()
        {
            //Arrange
            string input = "Sofia \"Slaveykov 1\"";

            Location expected = new Location("Sofia", "Slaveykov 1");

            //Act
            DifferentStatesReader reader = new DifferentStatesReader(new StringReader(input));
            Result<Location> result = reader.ReadLocation(input);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
            Assert.Equal(expected, result.Data!);

        }

        [Theory]
        [InlineData("Sofia \"Slaveykov 1\"")]
        [InlineData("Burgas \"Ivan Vazov 3\"")]
        [InlineData("Varna \"Baba tonka 50\"")]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<Location> result = reader.ReadLocation(input);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("alabala")]
        [InlineData("Sofia \"Slaveykov 1\" \"Slaveykov 1\"")]
        [InlineData("Sofia \"Slaveykov 1\" \"\"\"")]
        [InlineData("No No \"Slaveykov 1\"  ")]
        [InlineData("Meltovsa \"Slaveykov 1\" aaaaaaaaa")]
        [InlineData("\"Slaveykov 1\" Sofia")]        
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<Location> result = reader.ReadLocation(input);

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
