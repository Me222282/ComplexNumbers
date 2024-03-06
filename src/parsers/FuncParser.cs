namespace maths
{
    public class FuncParser : IWordParser
    {
        public IExpression Expression { get; private set; }
        public Operator Op => Operator.Function;
        public int Bracket => -1;

        public void Reset()
        {
            Expression = null;
        }
        public bool TryParse(Keyword word)
        {
            string str = word.Word.ToLower();
            
            switch (str)
            {
                case "sin":
                    Expression = Function.Sin;
                    break;
                case "cos":
                    Expression = Function.Cos;
                    break;
                case "tan":
                    Expression = Function.Tan;
                    break;
                case "sec":
                    Expression = Function.Sec;
                    break;
                case "cosec":
                case "csc":
                    Expression = Function.Cosec;
                    break;
                case "cot":
                    Expression = Function.Cot;
                    break;
                case "sinh":
                    Expression = Function.Sinh;
                    break;
                case "cosh":
                    Expression = Function.Cosh;
                    break;
                case "tanh":
                    Expression = Function.Tanh;
                    break;
                case "sech":
                    Expression = Function.Sech;
                    break;
                case "cosech":
                case "csch":
                    Expression = Function.Cosech;
                    break;
                case "coth":
                    Expression = Function.Coth;
                    break;
                    
                default:
                    return false;
            }
            
            return true;
        }
    }
}