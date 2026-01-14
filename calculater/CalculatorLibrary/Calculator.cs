using Newtonsoft.Json;
using System;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        private int count;
        private List<Operation> operations;
        private List<CompletedOperation> lastOperations;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();

            lastOperations = new();
            operations = new();

            AddOperations();
        }

        public List<string> GetAllOperations()
        {
            return operations.Select(o => o.Name).ToList();
        }
        public List<string> GetOperationsWithOneOperand()
        {
            return operations.Where(o => o.OneOperand == true).Select(o => o.Name).ToList();
        }
        public int Count()
        {
            return count;
        }
        public void ClearHistory()
        {
            lastOperations.Clear();
        }
        public List<string> GetHistory()
        {
            return lastOperations.Select(op => $"{op.Description}= {op.Result}").ToList();
        }
        public double DoOperation(double num1, double num2, string op)
        {
            var result = DoOperationTwoNumbers(num1, num2, op);
            var operation = CompletedOperation<double, double> (num1, num2, op, result);
            lastOperations.Add(operation);
            WriteLog(operation.Name, result, num1, num2);

            return result;
        }
        public double DoOperation(int indexOperation, double num, string op)
        {
            if (indexOperation < 0 || indexOperation > lastOperations.Count)
            {
                throw new Exception("The operation was not found in the log");
            }

            var lastOperation = lastOperations[indexOperation];
            var result = DoOperationTwoNumbers(lastOperation.Result, num, op);
            var operation = CompletedOperation<string, double>(lastOperation.Description, num, op, result);
            lastOperations.Add(operation);
            WriteLog(operation.Name, result, num);

            return result;
        }
        public double DoOperation(double num, string op)
        {
            var result = DoOperationOneNumber(num, op);
            var operation = Operation<double>(num, op, result);
            lastOperations.Add(operation);
            WriteLog(operation.Name, result, num);

            return result;
        }
        public double DoOperation(int indexOperation, string op)
        {
            if (indexOperation < 0 || indexOperation > lastOperations.Count)
            {
                throw new Exception("The operation was not found in the log");
            }

            var lastOperation = lastOperations[indexOperation];
            var result = DoOperationOneNumber(lastOperation.Result, op);
            var operation = Operation<string>(lastOperation.Description, op, result); 
            lastOperations.Add(operation);
            WriteLog(operation.Name, result, lastOperation.Result);
            return result;
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
        private void AddOperations()
        {
            operations.Add(new Operation("add", "Addition"));
            operations.Add(new Operation("sub", "Subtraction"));
            operations.Add(new Operation("mul", "Multiplication"));
            operations.Add(new Operation("div", "Division"));
            operations.Add(new Operation("sqrt", "Square root", true));
            operations.Add(new Operation("pow", "Exponentiation"));
            operations.Add(new Operation("sin", "Sine", true));
            operations.Add(new Operation("cos", "Cosine", true));
        }
        private double DoOperationTwoNumbers(double num1, double num2, string op)
        {
            double result = double.NaN;
            switch (op)
            {
                case "add":
                    result = num1 + num2;
                    break;
                case "sub":
                    result = num1 - num2;
                    break;
                case "mul":
                    result = num1 * num2;
                    break;
                case "div":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    break;
                case "pow":
                    result = Math.Pow(num1, num2);
                    break;
                default:
                    break;
            }
            count++;
            return result;
        }
        private double DoOperationOneNumber(double num, string op)
        {
            double result = double.NaN;
            switch (op)
            {
                case "sqrt":
                    result = Math.Sqrt(num);
                    break;
                case "sin":
                    result = Math.Sin(num);
                    break;
                case "cos":
                    result = Math.Cos(num);
                    break;
                default:
                    break;
            }
            count++;
            return result;
        }
        private void WriteLog(string nameOp, double result, double num1, double num2 = double.NaN)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            if (num2 != double.NaN)
            {
                writer.WritePropertyName("Operand2");
                writer.WriteValue(num2);
            }
            writer.WritePropertyName("Operation");
            writer.WriteValue(nameOp);
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
        }
        private CompletedOperation CompletedOperation<T, P>(T operand1, P operand2, string op, double result)
        {
            var description = "";
            var name = "";
            switch (op)
            {
                case "add":
                    description = $"{operand1} + {operand2} ";
                    name = "Add";
                    break;
                case "sub":
                    description = $"{operand1} - {operand2} ";
                    name = "Subtract";
                    break;
                case "mul":
                    description = $"{operand1} * {operand2} ";
                    name = "Multiply";
                    break;
                case "div":
                    description = $"{operand1} / {operand2} ";
                    name = "Divide";
                    break;
                case "pow":
                    description = $"{operand1} ^ {operand2} ";
                    name = "Pow";
                    break;
                default:
                    break;
            }
            return new CompletedOperation(name, description, result);
        }
        private CompletedOperation Operation<T>(T operand, string op, double result)
        {
            var description = "";
            var name = "";
            switch (op)
            {
                case "sqrt":
                    description = $"square root {operand} ";
                    name = "Sqrt";
                    break;
                case "sin":
                    description = $"sin {operand} ";
                    name = "Sin";
                    break;
                case "cos":
                    description = $"cos {operand} ";
                    name = "Cos";
                    break;
                default:
                    break;
            }
            return new CompletedOperation(name, description, result);
        }
    }
}