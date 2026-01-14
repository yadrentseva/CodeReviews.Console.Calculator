namespace CalculatorLibrary
{
    internal class Operation
    {
        internal string Name { get; }
        internal string FullName { get; }
        internal bool OneOperand { get; }
        public Operation(string name, string fullName, bool oneOperand = false)
        {
            Name = name;
            FullName = fullName;
            OneOperand = oneOperand; 
        }
    }
}
