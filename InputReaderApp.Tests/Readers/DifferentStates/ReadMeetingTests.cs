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
    public class ReadMeetingTests
    {
        [Fact]
        public void Read_ShouldReturnMeeting()
        {
            //Arrange
            string input = "Sofia 2h Galin Ivan Georgi";

            DataBase dataBase = new DataBase();

            dataBase.Employees.Add(new Employee("Galin", 36, 5000m));
            dataBase.Employees.Add(new Employee("Georgi", 24, 2500m));
            dataBase.Employees.Add(new Employee("Ivan", 40, 1000));

            dataBase.Locations.Add(new Location("Sofia", "Slaveykov 1"));
            dataBase.Locations.Add(new Location("Burgas", "Ivan Vazov 3"));
            dataBase.Locations.Add(new Location("Varna", "Baba tonka 50"));

            Meeting expected = new Meeting(dataBase.Locations[0],
                                             2f,
                                             new List<Employee>(){ dataBase.Employees[0],
                                                                    dataBase.Employees[2],
                                                                    dataBase.Employees[1]});

            //Act
            DifferentStatesReader reader = new DifferentStatesReader(new StringReader(input));
            Result<Meeting> result = reader.ReadMeeting(input, dataBase);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
            AssertExtensions.EqualMeetings(expected, result.Data!);

        }

        [Theory]
        [InlineData("Varna 1h Ivan Galin")]
        [InlineData("Varna 1h Ivan")]
        [InlineData("Burgas 2.4h Georgi Galin")]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Arrange
            DataBase dataBase = new DataBase();

            dataBase.Employees.Add(new Employee("Galin", 36, 5000m));
            dataBase.Employees.Add(new Employee("Georgi", 24, 2500m));
            dataBase.Employees.Add(new Employee("Ivan", 40, 1000));

            dataBase.Locations.Add(new Location("Sofia", "Slaveykov 1"));
            dataBase.Locations.Add(new Location("Burgas", "Ivan Vazov 3"));
            dataBase.Locations.Add(new Location("Varna", "Baba tonka 50"));
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<Meeting> result = reader.ReadMeeting(input,dataBase);

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("alabala")]
        [InlineData("Varna 1h")]
        [InlineData("Sofia! 2h Galin Ivan Georgi")]
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
