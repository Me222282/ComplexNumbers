namespace maths
{
    public class NumberParser : IWordParser
    {
        public IExpression Expression => new Term(_value, 0d);
        public Operator Op => Operator.NA;
        public int Bracket => -1;
        
        private double _value;
        
        public bool TryParse(Keyword word) => double.TryParse(word.Word, out _value);
        public void Reset() { _value = 0d; }
    }
    public class XParser : IWordParser
    {
        public IExpression Expression => new Term(1d, 1d);
        public Operator Op => Operator.NA;
        public int Bracket => -1;
        
        public bool TryParse(Keyword word)
        {
            string str = word.Word.Trim();
            
            return str.Length == 1 && char.ToLower(str[0]) == 'x';
        }
        public void Reset() { }
    }
    public class OperatorParser : IWordParser
    {
        public IExpression Expression => null;
        public Operator Op { get; private set; }
        public int Bracket => -1;
        
        public bool TryParse(Keyword word)
        {
            string str = word.Word.Trim();
            if (str.Length != 1) { return false; }
            char c = str[0];
            
            switch (c)
            {
                case '+':
                    Op = Operator.Add;
                    return true;
                case '-':
                    Op = Operator.Subtract;
                    return true;
                case '/':
                    Op = Operator.Divide;
                    return true;
                case '*':
                    Op = Operator.Multiply;
                    return true;
                case '^':
                    Op = Operator.Index;
                    return true;
            }
            
            return false;
        }
        public void Reset() { Op = Operator.NA; }
    }
    public class BracketParser : IWordParser
    {
        public IExpression Expression => null;
        public Operator Op => Operator.NA;
        public int Bracket { get; private set; }

        public bool TryParse(Keyword word)
        {
            Bracket = word.Bracket;
            return Bracket >= 0;
        }

        public void Reset() { Bracket = -1; }
    }
}