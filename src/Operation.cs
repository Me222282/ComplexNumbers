using System;

namespace maths
{
    public enum Operator : byte
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Index,
        Logarithm,
        Function
    }
    
    public class Operation : IExpression
    {
        public Operation(IExpression l, IExpression r, Operator o, bool n = false)
        {
            Left = l;
            Right = r;
            Operator = o;
            Negate = n;
        }
        
        public IExpression Left;
        public IExpression Right;
        
        public Operator Operator;

        public bool Negate { get; set; }

        public Complex Calculate(Complex x)
        {
            double m = Negate ? -1d : 1d;
            
            if (Operator == Operator.Function)
            {
                return m * Left.Calculate(Right.Calculate(x));
            }
            
            Complex l = Left.Calculate(x);
            Complex r = Right.Calculate(x);
            
            return m * (Operator switch
            {
                Operator.Add => l + r,
                Operator.Subtract => l - r,
                Operator.Multiply => l * r,
                Operator.Divide => l / r,
                Operator.Index => throw new Exception(), //l ^ r,
                Operator.Logarithm => throw new Exception(), //Math.Log(r, l),
                _ => l
            });
        }

        public bool IsConstant()
        {
            bool l = Left.IsConstant();
            bool r = Right.IsConstant();
            
            if (Operator == Operator.Function) { return r || l; }
            
            return l && r;
        }
        
        public static Operation operator *(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Multiply);
        }
        public static Operation operator /(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Divide);
        }
        public static Operation operator +(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Add);
        }
        public static Operation operator -(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Subtract);
        }
        public static Operation operator -(Operation e)
        {
            e.Negate = !e.Negate;
            return e;
        }
        public static Operation operator ^(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Index);
        }
        public static Operation operator <(Operation l, IExpression r)
        {
            return new Operation(l, r, Operator.Function);
        }
        public static Operation operator >(Operation l, IExpression r)
        {
            return new Operation(r, l, Operator.Function);
        }
    }
}
