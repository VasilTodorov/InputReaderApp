using InputReaderApp.Readers;
using InputReaderApp.Tests.Helpers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers.DifferentStates
{
    public class ReadEmployeeTests
    {
        [Fact]
        public void Read_ShouldReturnEmployee()
        {
            //Arrange
            string input = "Galin 36 5000";

            Employee expected = new Employee("Galin", 36, 5000m); 
                    

            //Act
            DifferentStatesReader reader = new DifferentStatesReader(new StringReader(input));
            Result<Employee> result = reader.ReadEmployee(input);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
            Assert.Equal(expected, result.Data!);

        }

        [Theory]
        [InlineData("G 3 50")]
        [InlineData("Vasil 2323     12")]
        [InlineData("   Tania   12  1")]        
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<Employee> result = reader.ReadEmployee(input);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("alabala")]
        [InlineData("Galin two 1")]
        [InlineData("Vasil 21 1a")]
        [InlineData("Bob")]        
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<Employee> result = reader.ReadEmployee(input);

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
