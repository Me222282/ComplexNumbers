using System;

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
        Exp,
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
        public int Presision { get; set; } = 20;
        
        public Complex Calculate(Complex x)
        {
            double m = Negate ? -1d : 1d;
            return m * (Option switch
            {
                FunctionType.Cos => Program.Cos(x, Presision),
                FunctionType.Sin => Program.Sin(x, Presision),
                FunctionType.Tan => Program.Sin(x, Presision) / Program.Cos(x, Presision),
                FunctionType.Arcsin => throw new Exception(),//Math.Asin(x),
                FunctionType.Arccos => throw new Exception(),//Math.Acos(x),
                FunctionType.Arctan => throw new Exception(),//Math.Atan(x),
                FunctionType.Sinh => Program.Sinh(x, Presision),
                FunctionType.Cosh => Program.Cos(x, Presision),
                FunctionType.Tanh => Program.Sinh(x, Presision) / Program.Cosh(x, Presision),
                FunctionType.Arcsinh => throw new Exception(),//Math.Asinh(x),
                FunctionType.Arccosh => throw new Exception(),//Math.Acosh(x),
                FunctionType.Arctanh => throw new Exception(),//Math.Atanh(x),
                FunctionType.Ln => throw new Exception(),//Math.Log(x),
                FunctionType.Exp => Program.Exp(x, Presision),
                _ => x
            });
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
        public static Function Sinh { get; } = new Function(FunctionType.Sinh);
        public static Function Cosh { get; } = new Function(FunctionType.Cosh);
        public static Function Tanh { get; } = new Function(FunctionType.Tanh);
        public static IExpression Sec { get; } = new Operation(new Term(1d, -1d), Function.Cos, Operator.Function);
        public static IExpression Cosec { get; } = new Operation(new Term(1d, -1d), Function.Sin, Operator.Function);
        public static IExpression Cot { get; } = new Operation(new Term(1d, -1d), Function.Tan, Operator.Function);
        public static IExpression Sech { get; } = new Operation(new Term(1d, -1d), Function.Cosh, Operator.Function);
        public static IExpression Cosech { get; } = new Operation(new Term(1d, -1d), Function.Sinh, Operator.Function);
        public static IExpression Coth { get; } = new Operation(new Term(1d, -1d), Function.Tanh, Operator.Function);
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
}