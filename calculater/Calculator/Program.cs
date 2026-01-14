using CalculatorLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        var calculator = new Calculator();

        while (!endApp)
        {
            double cleanNum1 = 0;
            double cleanNum2 = 0;
            double result = 0;

            ShowOperators(calculator);
            Console.Write("Your option? ");
            string? op = Console.ReadLine();
            while (op == null || !calculator.GetAllOperations().Contains(op))
            {
                Console.Write("This is not valid operator. Please enter again: ");
                op = Console.ReadLine();
            }
            var onlyOneOperand = calculator.GetOperationsWithOneOperand().Contains(op);

            Console.Write("Do you want to take a result from a history: yes or no? ");
            string? source = Console.ReadLine();
            while (source != "yes" && source !="no")
            {
                Console.Write("This is not valid answer. Please enter again: ");
                source = Console.ReadLine();
            }
            var firstOperandFromHistory = source == "yes";
            int indexOperation = 0;

            if (firstOperandFromHistory)
            {
                var lastOperations = calculator.GetHistory();
                if (lastOperations.Count == 0)
                {
                    Console.WriteLine("History is empty.");
                    firstOperandFromHistory = false;
                    cleanNum1 = InputNumber();
                }
                else
                {
                    indexOperation = InputNumberRecordHistory(lastOperations);
                }
            }
            else
            {
                cleanNum1 = InputNumber();
            }

            if (!onlyOneOperand)
            {
                cleanNum2 = InputNumber(true);
            }

            try
            {
                if (onlyOneOperand)
                {
                    if (firstOperandFromHistory)
                    {
                        result = calculator.DoOperation(indexOperation, op);
                    }
                    else
                    {
                        result = calculator.DoOperation(cleanNum1, op);
                    }
                }
                else
                {
                    if (firstOperandFromHistory)
                    {
                        result = calculator.DoOperation(indexOperation, cleanNum2, op);
                    }
                    else
                    {
                        result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    }
                }
                if (double.IsNaN(result))
                {
                    Console.WriteLine("This operation will result in a mathematical error.\n");
                }
                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }

            Console.WriteLine("------------------------\n");
            Console.Write("Press 'n' and Enter to close the app, 'v' to view history, 'd' to delete history or press any other key and Enter to continue: ");
            var answ = Console.ReadLine();
            if (answ == "n") endApp = true;
            while (answ == "v" || answ == "d")
            {
                if (answ == "v") ShowHistory(calculator);
                if (answ == "d") calculator.ClearHistory();
                Console.Write("Press 'n' and Enter to close the app, 'v' to view history, 'd' to delete history or press any other key and Enter to continue: ");
                answ = Console.ReadLine();
            }
            Console.WriteLine("\n");
        }

        calculator.Finish();

        return;
    }

    static void ShowOperators(Calculator calculator)
    {
        Console.WriteLine("Choose an operator from the following list:");
        var operations = calculator.GetAllOperations();
        foreach (var op in operations)
        {
            Console.WriteLine(op);
        }
    }
    static int InputNumberRecordHistory(List<string> lastOperations)
    {
        var index = 1;
        foreach (var record in lastOperations)
        {
            Console.WriteLine($"{index} {record}");
            index++;
        }

        Console.Write("Enter the record number: ");
        string? numberRecords = Console.ReadLine();
        int indexOperation;
        while (int.TryParse(numberRecords, out indexOperation) && (indexOperation > lastOperations.Count || indexOperation <= 0))
        {
            Console.Write("This is not valid input. Please enter again: ");
            numberRecords = Console.ReadLine();
        }
        return --indexOperation;
    }

    private static void ShowHistory(Calculator calculator)
    {
        var lastOperations = calculator.GetHistory();
        var index = 1;
        Console.WriteLine("Last operations:"); 
        foreach (var record in lastOperations)
        {
            Console.WriteLine($"{index} {record}");
            index++;
        }
    }
    private static double InputNumber(bool secondNumber = false)
    {
        var path = secondNumber == true ? "second " : "";
        Console.Write($"Type a {path} number, and then press Enter: ");
        var numInput = Console.ReadLine();

        double cleanNum = 0;
        while (!double.TryParse(numInput, out cleanNum))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput = Console.ReadLine();
        }
        return cleanNum;
    }
}