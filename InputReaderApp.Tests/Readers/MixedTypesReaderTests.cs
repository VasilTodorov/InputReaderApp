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
    public class MixedTypesReaderTests
    {
        [Fact]
        public void Read_ShouldReturnMixedTypesList_1()
        {
            //Arrange
            string input = "Alice 23 91.5" + "\n" +
                            "Bob 30 88.0" + "\n" +
                            "Charlie 25 95.2";

            List<Person> expected = new List<Person>();

            expected.Add(new Person("Alice", 23, 91.5f));
            expected.Add(new Person("Bob", 30, 88.0f));
            expected.Add(new Person("Charlie", 25, 95.2f));
            
            //Act
            MixedTypesReader reader = new MixedTypesReader(new StringReader(input));
            Result<List<Person>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.EqualLists(expected, result.Data!);

        }

        [Fact]
        public void Read_ShouldReturnMixedTypesList_2()
        {
            //Arrange
            string input = "Galin 36 75.5" + "\n" +
                            "Vladislav 31 84.123" + "\n" +
                            "Charlie 25 95.2" + "\n" +
                            "Gregore 42 120.4";

            List<Person> expected = new List<Person>();

            expected.Add(new Person("Galin", 36, 75.5f));
            expected.Add(new Person("Vladislav", 31, 84.123f));
            expected.Add(new Person("Charlie", 25, 95.2f));
            expected.Add(new Person("Gregore", 42, 120.4f));

            //Act
            MixedTypesReader reader = new MixedTypesReader(new StringReader(input));
            Result<List<Person>> result = reader.Read();

            //Assert
            Assert.True(result.IsSuccess);
            AssertExtensions.EqualLists(expected, result.Data!);

        }

        [Theory]
        [InlineData("Alabala")]
        [InlineData("3 2 1")]
        [InlineData("Vasil 2 3 4")]
        [InlineData("Toni -22 87.9")]
        [InlineData("Alice 2.3 2")]
        public void Read_ShouldFail_OnVariousInvalidInputs(string input)
        {
            //Act
            var reader = new MixedTypesReader(new StringReader(input));
            Result<List<Person>> result = reader.Read();

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
        }
    }
}
