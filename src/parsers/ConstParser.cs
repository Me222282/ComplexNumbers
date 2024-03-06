using System;

namespace maths
{
    public class ConstParser : IWordParser
    {
        public IExpression Expression => new Term(_const, 0d);
        public Operator Op => Operator.NA;
        public int Bracket => -1;
        
        private Complex _const;
        
        public void Reset() { _const = 0d; }

        public bool TryParse(Keyword word)
        {
            string str = word.Word.Trim().ToLower();
            
            switch (str)
            {
                case "e":
                    _const = Math.E;
                    break;
                case "pi":
                case "π":
                    _const = Math.PI;
                    break;
                case "tau":
                case "Τ":
                case "τ":
                    _const = Math.Tau;
                    break;
                case "i":
                    _const = (I)1d;
                    break;
                
                default:
                    return false;
            }
            
            return true;
        }
    }
}