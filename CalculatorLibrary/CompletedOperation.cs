namespace CalculatorLibrary
{
    internal class CompletedOperation
    {
        internal string Name { get; }
        internal string Description { get; }
        internal double Result { get; }
        internal CompletedOperation(string name, string desc, double result)
        {
            Name = name; 
            Description = desc;
            Result = result;
        }
    }
}
