using System;

namespace maths
{
    public class Term : IExpression
    {
        public Term(Complex c, double p)
        {
            Coefficient = c;
            Power = p;
        }
        public Term() : this(1, 1) { }
        
        public Complex Coefficient { get; set; }
        public double Power { get; set; }
        
        public bool Negate
        {
            get => double.IsNegative(Coefficient.R);
            set
            {
                //double c = Math.Abs(Coefficient);
                
                //Coefficient = value ? -c : c;
            }
        }
        
        public Complex Calculate(Complex x)
        {
            if (Power == 0d) { return Coefficient; }
            if (x == 0d) { return 0d; }
            
            return Coefficient * (x ^ (int)Power);
        }
        public IExpression Differentiate()
        {
            return new Term(Coefficient * Power, Power - 1);
        }
        public IExpression Integrate()
        {
            return new Term(Coefficient / (Power + 1), Power + 1);
        }
        
        public bool IsConstant() => Coefficient == 0d || Power == 0d;
        
        public static Term PI { get; } = new Term(Math.PI, 0d);
        public static Term E { get; } = new Term(Math.E, 0d);
        
        public static Operation operator *(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Multiply);
        }
        public static Operation operator /(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Divide);
        }
        public static Operation operator +(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Add);
        }
        public static Operation operator -(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Subtract);
        }
        public static Operation operator ^(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Index);
        }
        public static Operation operator <(Term l, IExpression r)
        {
            return new Operation(l, r, Operator.Function);
        }
        public static Operation operator >(Term l, IExpression r)
        {
            return new Operation(r, l, Operator.Function);
        }
    }
}
