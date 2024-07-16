namespace CommonLibrary
{
    public static class ExpressionValidator
    {
        public static bool IsValidExpression(string expression)
        {
            expression = expression.Replace(",", ".");

            int balance = 0;
            foreach (char c in expression)
            {
                if (c == '(')
                {
                    balance++;
                }
                else if (c == ')')
                {
                    balance--;
                }

                if (balance < 0)
                {
                    return false; // closing bracket without matching opening bracket
                }
            }

            return balance == 0; // all opening brackets must be closed
        }
    }
}