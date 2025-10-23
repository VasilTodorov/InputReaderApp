using InputReaderApp.Utils;
using InputReaderApp.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Workers
{
    public class JSONFileTableWriterTests
    {
        private readonly string testFilePath = "test_output.txt";

        [Fact]
        public void Write_ValidJson_WritesTableToFile()
        {
            // Arrange
            var writer = new JSONFileTableWriter();
            string json = @"[
            { ""Name"": ""Alice"", ""Age"": 30, ""City"": ""London"" },
            { ""Name"": ""Bob"", ""Age"": 25, ""City"": ""New York"" }
        ]";

            // Expected table string
            string expectedTable =
                "Name\t|Age\t|City" + Environment.NewLine +
                "----\t|---\t|----" + Environment.NewLine +
                "Alice\t|30\t|London" + Environment.NewLine +
                "Bob\t|25\t|New York" + Environment.NewLine;

            // Act
            var result = writer.Write(testFilePath,json);

            // Assert
            Assert.True(result.IsSuccess, "Write should succeed");
            Assert.True(File.Exists(testFilePath), "File should exist");

            string actualContent = File.ReadAllText(testFilePath);
            Assert.Equal(expectedTable, actualContent);

            // Clean up
            if (File.Exists(testFilePath))
                File.Delete(testFilePath);
        }

        [Fact]
        public void Write_EmptyJson_ReturnsInvalidFormatError()
        {
            // Arrange
            var writer = new JSONFileTableWriter();
            string emptyJson = "[]";

            // Act
            var result = writer.Write(testFilePath, emptyJson);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.InvalidFormat, result.Code);
            Assert.False(File.Exists(testFilePath));
        }

        [Fact]
        public void Write_NullOutputPath_ReturnsInputNotFoundError()
        {
            // Arrange
            var writer = new JSONFileTableWriter();
            string json = @"[{""Name"":""Alice""}]";

            // Act
            var result = writer.Write(null!,json );

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.InputNotFound, result.Code);
        }
    }
}
