using System;

namespace maths
{
    public interface IExpression
    {
        public bool Negate { get; set; }
        public Complex Calculate(Complex x);
        
        public bool IsConstant();
        
        public static Operation operator *(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Multiply);
        }
        public static Operation operator /(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Divide);
        }
        public static Operation operator +(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Add);
        }
        public static Operation operator -(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Subtract);
        }
        public static Operation operator ^(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Index);
        }
        public static Operation operator <(IExpression l, IExpression r)
        {
            return new Operation(l, r, Operator.Function);
        }
        public static Operation operator >(IExpression l, IExpression r)
        {
            return new Operation(r, l, Operator.Function);
        }
        
        public IExpression Simplify()
        {
            if (this is Operation op)
            {
                return op.Simplify();
            }
            
            return this;
        }
    }
}
