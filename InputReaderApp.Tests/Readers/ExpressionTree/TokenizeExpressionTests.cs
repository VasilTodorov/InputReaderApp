using InputReaderApp.Readers;
using InputReaderApp.Tests.Helpers;
using InputReaderApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Tests.Readers.ExpressionTree
{
    public class TokenizeExpressionTests
    {
        [Fact]
        public void Read_ShouldReturnList()
        {
            //Arrange
            string input = "3 + 5 * (2 - 1)";

            List<string> expected = new List<string> {"3", "+", "5", "*", "(", "2", "-", "1", ")"};

            //Act
            ExpressionReader reader = new ExpressionReader(new StringReader(input));
            List<string> result = reader.TokenizeExpression("3 + 5 * (2 - 1)");

            //Assert
            AssertExtensions.EqualLists(expected, result);

        }
        [Theory]
        [InlineData("3+5*(2-1)", new[] { "3", "+", "5", "*", "(", "2", "-", "1", ")" })]
        [InlineData("3 + 5 * 2 - 1", new[] { "3", "+", "5", "*", "2", "-", "1" })]
        [InlineData("3 + 5 * 2 * 4 / 2 - 1", new[] { "3", "+", "5", "*", "2", "*", "4", "/", "2", "-", "1" })]
        [InlineData("((3 + 5 * 2) * 4 )/ 2 - 1", new[] { "(", "(", "3", "+", "5", "*", "2", ")", "*", "4", ")", "/", "2", "-", "1" })]
        public void Read_ShouldSucceed_OnVariousValidInputs(string input, string[] expected)
        {
            //Arrange
            var reader = new ExpressionReader(new StringReader(input));

        // Act
        List<string> result = reader.TokenizeExpression(input);

        // Assert
        AssertExtensions.EqualLists(expected.ToList(), result);
        }

        [Theory]
        [InlineData("3+5*(2-1 1)", new[] { "3", "+", "5", "*", "(", "2", "-", "1", "1", ")" })]
        [InlineData("3 + 5 * 2 2 - 1", new[] { "3", "+", "5", "*", "2", "2", "-", "1" })]
        [InlineData("ala 3 + 5 * 2 * 4 / 2 - 1", new[] { "ala", "3", "+", "5", "*", "2", "*", "4", "/", "2", "-", "1" })]
        [InlineData("((3 + 5 * 2))) * 4 )/ 2 - 1", new[] { "(", "(", "3", "+", "5", "*", "2", ")", ")", ")", "*", "4", ")", "/", "2", "-", "1" })]
        public void Read_ShouldSucceed_OnVariousInvalidInputs(string input, string[] expected)
        {
            //Arrange
            var reader = new ExpressionReader(new StringReader(input));

            // Act
            List<string> result = reader.TokenizeExpression(input);

            // Assert
            AssertExtensions.EqualLists(expected.ToList(), result);
        }
    }
}
