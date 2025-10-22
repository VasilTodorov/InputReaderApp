using InputReaderApp.Readers;
using InputReaderApp.Tests.Helpers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers.DifferentStates
{
    public class DifferentStatesReaderTests
    {
        [Fact]
        public void Read_ShouldReturnDifferentStatesDataBase()
        {
            //Arrange
            string input = "<Employees>\r\n" +
                            "Galin 36 5000\r\n" +
                            "Georgi 24 2500\r\n" +
                            "Ivan 40 1000\r\n\r\n" +
                            "<Meetings>\r\n" +
                            "Sofia 2h Galin Ivan Georgi\r\n" +
                            "Varna 1h Ivan Galin\r\n\r\n\r\n" +
                            "<Locations>\r\n" +
                            "Sofia \"Slaveykov 1\"\r\n" +
                            "Burgas \"Ivan Vazov 3\"\r\n" +
                            "Varna \"Baba tonka 50\"";

            DataBase expected = new DataBase();
            List<Employee> employees = new List<Employee>();
            List<Location> locations = new List<Location>();
            List<Meeting> meeting = new List<Meeting>();

            employees.Add(new Employee("Galin", 36, 5000m));
            employees.Add(new Employee("Georgi", 24, 2500m));
            employees.Add(new Employee("Ivan", 40, 1000));

            locations.Add(new Location("Sofia", "Slaveykov 1"));
            locations.Add(new Location("Burgas", "Ivan Vazov 3"));
            locations.Add(new Location("Varna", "Baba tonka 50"));

            meeting.Add(new Meeting(locations[0],
                                             2f,
                                             new List<Employee>(){ employees[0],
                                                                    employees[2],
                                                                    employees[1]}));
            meeting.Add(new Meeting(locations[2],
                                             1f,
                                             new List<Employee>(){ employees[2],
                                                                    employees[0]
                                                                    }));
            expected.Employees = employees;
            expected.Locations = locations;
            expected.Meetings = meeting;


            //Act
            DifferentStatesReader reader = new DifferentStatesReader(new StringReader(input));
            Result<DataBase> result = reader.Read();

            //Assert            
            Assert.True(result.IsSuccess, result.Message);
            AssertExtensions.EqualDataBase(expected, result.Data!);
        }

        [Theory]
        [InlineData("<Employees>\r\n<Meetings>\r\n<Locations>\r\n")]
        [InlineData("<Meetings>\r\n<Employees>\r\n<Locations>\r\n")]
        [InlineData(
                            "<Locations>\r\n" +
                            "Sofia \"Slaveykov 1\"\r\n" +
                            "Burgas \"Ivan Vazov 3\"\r\n" +
                            "Varna \"Baba tonka 50\"\r\n" +
                           "<Employees>\r\n" +
                            "Galin 36 5000\r\n" +
                            "Georgi 24 2500\r\n" +
                            "Ivan 40 1000\r\n\r\n" +
                            "<Meetings>\r\n" +
                            "Sofia 2h Galin Ivan Georgi\r\n" +
                            "Varna 1h Ivan Galin\r\n\r\n\r\n")]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<DataBase> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess, result.Message);
        }

        [Theory]
        [InlineData("<Employees>\r\n<Employees>\r\n<Meetings>\r\n<Location>\r\n")]
        [InlineData("<Meetings>\r\n<Location>\r\n")]
        [InlineData("<Employees>\r\n" +
                            "Galin 36 5000\r\n" +
                            "Georgi 24 2500\r\n" +
                            "Ivan 40 1000\r\n\r\n" +
                            "<Meetings>\r\n" +
                            "Sofia 2h Galin Ivan Toni\r\n" +
                            "Varna 1h Ivan Galin\r\n\r\n\r\n" +
                            "<Locations>\r\n" +
                            "Sofia \"Slaveykov 1\"\r\n" +
                            "Burgas \"Ivan Vazov 3\"\r\n" +
                            "Varna \"Baba tonka 50\"")]
        [InlineData(
                    "<Employees>\r\n" +
                    "Galin 36 5000\r\n" +
                    "Georgi 24 2500\r\n" +
                    "Ivan 40 1000\r\n\r\n" +
                    "<Meetings>\r\n" +
                    "Sofia 2h Galin Ivan Georgi\r\n" +
                    "Lulin 1h Ivan Galin\r\n\r\n\r\n" +
                    "<Locations>\r\n" +
                    "Sofia \"Slaveykov 1\"\r\n" +
                    "Burgas \"Ivan Vazov 3\"\r\n" +
                    "Varna \"Baba tonka 50\"")]
        [InlineData(
                    "<Employees>\r\n" +
                    "Galin 36 5000\r\n" +
                    "Georgi 24 2500\r\n" +
                    "Ivan 40 1000\r\n\r\n" +
                    "<Meetings>\r\n" +
                    "Sofia 2h Galin Ivan Georgi\r\n" +
                    "Varna 1h Ivan Galin\r\n\r\n\r\n" +
                    "<?Locations>\r\n" +
                    "Sofia \"Slaveykov 1\"\r\n" +
                    "Burgas \"Ivan Vazov 3\"\r\n" +
                    "Varna \"Baba tonka 50\"")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new DifferentStatesReader(new StringReader(input));
            Result<DataBase> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure, result.Message);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
