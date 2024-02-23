/*using System;

namespace maths
{
    public enum FunctionType : byte
    {
        Sin,
        Cos,
        Tan,
        Arcsin,
        Arccos,
        Arctan,
        Ln,
        Sinh,
        Cosh,
        Tanh,
        Arcsinh,
        Arccosh,
        Arctanh
    }

    public class Function : IExpression
    {
        public Function(FunctionType o, bool n = false)
        {
            Option = o;
            Negate = n;
        }
        
        public bool Negate { get; set; }
        public FunctionType Option { get; set; }
        
        public double Calculate(double x)
        {
            double m = Negate ? -1d : 1d;
            return m * (Option switch
            {
                FunctionType.Cos => Math.Cos(x),
                FunctionType.Sin => Math.Sin(x),
                FunctionType.Tan => Math.Tan(x),
                FunctionType.Arcsin => Math.Asin(x),
                FunctionType.Arccos => Math.Acos(x),
                FunctionType.Arctan => Math.Atan(x),
                FunctionType.Sinh => Math.Cosh(x),
                FunctionType.Cosh => Math.Sinh(x),
                FunctionType.Tanh => Math.Tanh(x),
                FunctionType.Arcsinh => Math.Asinh(x),
                FunctionType.Arccosh => Math.Acosh(x),
                FunctionType.Arctanh => Math.Atanh(x),
                FunctionType.Ln => Math.Log(x),
                _ => x
            });
        }

        public IExpression Differentiate()
        {
            return Option switch
            {
                FunctionType.Cos => new Function(FunctionType.Sin, !Negate),
                FunctionType.Sin => new Function(FunctionType.Cos, Negate),
                FunctionType.Tan => new Operation(
                        new Term(1d, -2d), Function.Cos, Operator.Function, Negate),
                //FunctionType.Arcsin => Math.Asin(x),
                //FunctionType.Arccos => Math.Acos(x),
                //FunctionType.Arctan => Math.Atan(x),
                //FunctionType.Sinh => Math.Cosh(x),
                //FunctionType.Cosh => Math.Sinh(x),
                //FunctionType.Tanh => Math.Tanh(x),
                //FunctionType.Arcsinh => Math.Asinh(x),
                //FunctionType.Arccosh => Math.Acosh(x),
                //FunctionType.Arctanh => Math.Atanh(x),
                FunctionType.Ln => new Term(Negate ? -1d : 1d, -1d),
                _ => this
            };
        }

        public IExpression Integrate()
        {
            throw new System.NotImplementedException();
        }
        
        public bool IsConstant() => false;

        public override bool Equals(object obj)
        {
            return obj is Function f &&
                f.Option == Option &&
                f.Negate == Negate;
        }

        public static Function Sin { get; } = new Function(FunctionType.Sin);
        public static Function Cos { get; } = new Function(FunctionType.Cos);
        public static Function Tan { get; } = new Function(FunctionType.Tan);
        public static IExpression Sec { get; } = new Operation(new Term(1d, -1d), Function.Cos, Operator.Function);
        public static IExpression Cosec { get; } = new Operation(new Term(1d, -1d), Function.Sin, Operator.Function);
        public static IExpression Cot { get; } = new Operation(new Term(1d, -1d), Function.Tan, Operator.Function);
        public static Function Ln { get; } = new Function(FunctionType.Ln);
        public static Function NSin { get; } = new Function(FunctionType.Sin, true);
        public static Function NCos { get; } = new Function(FunctionType.Cos, true);
        public static Function NTan { get; } = new Function(FunctionType.Tan, true);
        public static IExpression NSec { get; } = new Operation(new Term(1d, -1d), Function.Cos, Operator.Function, true);
        public static IExpression NCosec { get; } = new Operation(new Term(1d, -1d), Function.Sin, Operator.Function, true);
        public static IExpression NCot { get; } = new Operation(new Term(1d, -1d), Function.Tan, Operator.Function, true);
        public static Function NLn { get; } = new Function(FunctionType.Ln, true);
        
        public static Operation operator *(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Multiply);
        }
        public static Operation operator /(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Divide);
        }
        public static Operation operator +(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Add);
        }
        public static Operation operator -(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Subtract);
        }
        public static Operation operator ^(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Index);
        }
        public static Operation operator <(Function l, IExpression r)
        {
            return new Operation(l, r, Operator.Function);
        }
        public static Operation operator >(Function l, IExpression r)
        {
            return new Operation(r, l, Operator.Function);
        }
    }
}*/