using InputReaderApp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Readers
{
    public class ExpressionReader : ReaderBase<Double>
    {
        public ExpressionReader(TextReader? input = null) : base(input) { }
        public override Result<double> Read()
        {
            string? line = Input.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                return Result<double>.Fail(ErrorCode.InputNotFound, "Error(1) : input not found");

            return EvaluateExpression(line);
        }
        internal Result<double> EvaluateExpression(string expression)
        {
            Stack<string> operators = new Stack<string>();
            Stack<double> operands = new Stack<double>();

            string[] operatorSymbols = { "+", "-", "/", "*", "%", "(", ")" };
            if (!IsValidExpression(expression))
            {
                return Result<double>.Fail(ErrorCode.InvalidFormat, "Error(14): " + expression);
            }
            List<string> tokens = TokenizeExpression(expression);

            foreach (var token in tokens!)
            {
                if (!operatorSymbols.Contains(token))
                {
                    if (double.TryParse(token, out double number))
                        operands.Push(number);
                    else return Result<double>.Fail(ErrorCode.InvalidFormat, $"Error(3): {token} isn't a number");
                }
                else
                {

                    if (token == "(")
                    {
                        operators.Push(token);
                    }
                    else if (token == ")")
                    {
                        var result = CalculateTillLeftParenthesis(operands, operators);
                        if (result.IsFailure)
                            return Result<double>.Fail((ErrorCode)result.Code!, result.Message);
                    }
                    else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
                    {
                        var result = CalculateTillThereAreNoOperatorsWithGreaterOrEqualPriority(operands, operators, token);
                        if (result.IsFailure)
                            return Result<double>.Fail((ErrorCode)result.Code!, result.Message);
                    }
                    else
                    {
                        return Result<double>.Fail(ErrorCode.InvalidFormat, $"Error(8): no such operator - {token}");
                    }

                }
            }

            while (operators.Count > 0)
            {
                var result = Calculate(operands, operators);
                if (result.IsFailure)
                    return Result<double>.Fail((ErrorCode)result.Code!, "Error(10):" + result.Message);
            }

            if (operands.Count != 1)
            {
                return Result<double>.Fail(ErrorCode.InvalidFormat, $"Error(11): there must be only one operand but: - {operands.Count}");
            }
            return Result<double>.Success(operands.Pop());
        }
        internal List<string> TokenizeExpression(string expression)
        {
            expression = expression.Trim();
            List<string> result = new List<string>();

            char[] delimiters = { '+', '-', '/', '*', '%', '(', ')', ' ' };
            var numberTokens = expression.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int numberTokensIndex = 0;

            for (int i = 0; i < expression.Length; i++)
            {
                if (delimiters.Contains(expression[i]))
                {
                    if (expression[i] != ' ')
                        result.Add(expression[i].ToString());
                }
                else
                {
                    if (numberTokensIndex < numberTokens.Length)
                    {
                        result.Add(numberTokens[numberTokensIndex].Trim());
                        i += numberTokens[numberTokensIndex].Length - 1;
                        numberTokensIndex++;
                    }
                }
            }

            return result;
        }
        internal Result CalculateTillLeftParenthesis(Stack<double> operands, Stack<string> operators)
        {
            while (operators.Count > 0)
            {
                var topOperator = operators.Peek();
                if (topOperator == "(")
                {
                    operators.Pop();
                    return Result.Success();
                }
                var result = Calculate(operands, operators);
                if (result.IsFailure)
                    return Result.Fail((ErrorCode)result.Code!, result.Message);
            }
            return Result.Success();
        }
        internal Result CalculateTillThereAreNoOperatorsWithGreaterOrEqualPriority(Stack<double> operands, Stack<string> operators, string operatorToCompare)
        {
            if (!(operatorToCompare == "*" || operatorToCompare == "%" || operatorToCompare == "/" || operatorToCompare == "+" || operatorToCompare == "-"))
                return Result.Fail(ErrorCode.InvalidFormat, $"Error(15): operator to compare isn't an operator - {operatorToCompare}");
            while (operators.TryPeek(out var top))
            {
                if ((top == "+" || top == "-") &&
                    (operatorToCompare == "*" || operatorToCompare == "%" || operatorToCompare == "/"))
                    break;
                if ((top == "(" || top == ")"))
                    break;
                var result = Calculate(operands, operators);
                if (result.IsFailure)
                    return Result.Fail((ErrorCode)result.Code!, result.Message);
            }
            operators.Push(operatorToCompare);
            return Result.Success();
        }
        internal Result Calculate(Stack<double> operands, Stack<string> operators)
        {
            if (operands.Count < 2)
                return Result.Fail(ErrorCode.InvalidFormat, "Error(5): operands stack too small");
            if (operators.Count < 1)
                return Result.Fail(ErrorCode.InvalidFormat, "Error(6): operators stack too small");

            string topOperator = operators.Pop();
            double rightOperand = operands.Pop();
            double leftOperand = operands.Pop();
            double result;
            switch (topOperator)
            {
                case "+":
                    result = leftOperand + rightOperand;
                    break;
                case "-":
                    result = leftOperand - rightOperand;
                    break;
                case "/":
                    result = leftOperand / rightOperand;
                    break;
                case "*":
                    result = leftOperand * rightOperand;
                    break;
                case "%":
                    result = leftOperand % rightOperand;
                    break;
                default:
                    return Result.Fail(ErrorCode.InvalidFormat, $"Error(7): invalid operator - {topOperator}");
            }

            operands.Push(result);
            return Result.Success();
        }
        internal bool IsValidExpression(string expression)
        {
            List<string> tokens = TokenizeExpression(expression);
            int NoPairleftParenthesisCount = 0;
            string[] operatorSymbols = { "+", "-", "/", "*", "%" };

            bool OperatorSymbolShouldBeNext = false;
            bool OperandShouldBeNext = true;
            foreach (var token in tokens)
            {
                if (operatorSymbols.Contains(token))
                {
                    if (!OperatorSymbolShouldBeNext)
                        return false;
                    OperatorSymbolShouldBeNext = false;
                    OperandShouldBeNext = true;
                }
                else if (token == "(")
                {
                    if (OperatorSymbolShouldBeNext)
                        return false;
                    NoPairleftParenthesisCount++;
                }
                else if (token == ")")
                {
                    if (OperandShouldBeNext)
                        return false;
                    NoPairleftParenthesisCount--;
                    if (NoPairleftParenthesisCount < 0)
                        return false;
                }
                else
                {
                    if (!OperandShouldBeNext)
                        return false;
                    if (!double.TryParse(token, out double r))
                        return false;
                    OperandShouldBeNext = false;
                    OperatorSymbolShouldBeNext = true;
                }
            }
            if (NoPairleftParenthesisCount != 0)
                return false;
            return true;
        }
    }
}
