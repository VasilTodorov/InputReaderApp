using InputReaderApp.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Workers
{
    public class FileWriterTests
    {
        [Fact]
        public void Write_Should_Create_File_With_Content()
        {
            // Arrange
            var writer = new FileWriter();
            var filePath = Path.Combine(Path.GetTempPath(), "test_output.txt");
            var content = "Hello, world!";

            // Act
            var result = writer.Write(content, filePath);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(File.Exists(filePath));
            Assert.Equal(content, File.ReadAllText(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void Write_Should_Return_Failure_For_Invalid_Path()
        {
            // Arrange
            var writer = new FileWriter();
            string invalidPath = "";

            // Act
            var result = writer.Write("data", invalidPath);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("path", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Write_Should_Handle_Exception()
        {
            // Arrange
            var writer = new FileWriter();
            var invalidPath = "?:\\invalid\\path.txt"; // invalid characters

            // Act
            var result = writer.Write("data", invalidPath);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error writing", result.Message);
        }
    }
}
