using WebApiServer.Services;

namespace WebApiTests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData("2+3", 5)]
        [InlineData("10-4", 6)]
        [InlineData("2*3", 6)]
        [InlineData("8/2", 4)]
        [InlineData("2+3*4", 14)]
        [InlineData("(2+3)*4", 20)]
        [InlineData("2.5*2", 5)]
        [InlineData("10/2.5", 4)]
        public void EvaluateExpression_ValidExpressions_ReturnsExpectedResult(string expression, double expected)
        {
            // Act
            double result = Calculator.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("2+")]
        [InlineData("2/0")]
        [InlineData("2++2")]
        [InlineData("(2+3")]
        public void EvaluateExpression_InvalidExpressions_ThrowsArgumentException(string expression)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Calculator.EvaluateExpression(expression));
        }

        [Theory]
        [InlineData("2.5*2", 5)]
        [InlineData("2,5*2", 5)]
        public void EvaluateExpression_DecimalExpressions_ReturnsExpectedResult(string expression, double expected)
        {
            // Act
            double result = Calculator.EvaluateExpression(expression);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}