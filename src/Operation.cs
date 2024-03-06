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
        Function,
        NA
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
                Operator.Index => l ^ r,
                Operator.Logarithm => Program.Ln(r, 20) / Program.Ln(l, 20),
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
        
        public IExpression Simplify()
        {
            if (Left is Operation ol)
            {
                Left = ol.Simplify();
            }
            if (Right is Operation or)
            {
                Right = or.Simplify();
            }
            
            if (Operator == Operator.Function &&
                Right is Term t && t.Power == 1d && t.Coefficient == 1d)
            {
                return Left;
            }
            
            if (Left.IsConstant() && Right.IsConstant())
            {
                return new Term(Calculate(0d), 0d);
            }
            
            if (Operator == Operator.Multiply || Operator == Operator.Divide)
            {
                if (Left.IsConstant() && Left.Calculate(0d) == 1d)
                {
                    return Right;
                }
                if (Right.IsConstant() && Right.Calculate(0d) == 1d)
                {
                    return Left;
                }
            }
            if (Operator == Operator.Add || Operator == Operator.Subtract)
            {
                if (Left.IsConstant() && Left.Calculate(0d) == 0d)
                {
                    return Right;
                }
                if (Right.IsConstant() && Right.Calculate(0d) == 0d)
                {
                    return Left;
                }
            }
            
            if (Operator == Operator.Index && Left is Term e &&
                e.Coefficient == Math.E && e.Power == 0d)
            {
                if (Right is Term x && x.Power == 1d && x.Coefficient == 1d)
                {
                    return Function.Exp;
                }
                
                return Function.Exp < Right;
            }
            
            if (Operator == Operator.Index && Left is Term itl &&
                Right is Term itr && itr.IsConstant())
            {
                return new Term(itl.Coefficient ^ itr.Coefficient, itl.Coefficient * itr.Coefficient);
            }
            
            if (Left is Term tl && Right is Term tr)
            {
                if (Operator == Operator.Multiply)
                {
                    return new Term(tl.Coefficient * tr.Coefficient, tl.Power + tr.Power);
                }
                if (Operator == Operator.Divide)
                {
                    return new Term(tl.Coefficient / tr.Coefficient, tl.Power - tr.Power);
                }
                
                if (tl.Power != tr.Power)
                {
                    return this;
                }
                
                if (Operator == Operator.Add)
                {
                    return new Term(tl.Coefficient + tr.Coefficient, tl.Power);
                }
                if (Operator == Operator.Subtract)
                {
                    return new Term(tl.Coefficient - tr.Coefficient, tl.Power);
                }
            }
            
            return this;
        }

        public override string ToString()
        {
            if (Operator == Operator.Function)
            {
                return Left.ToString().Replace("x", Right.ToString());
            }
            
            char c = Operator switch
            {
                Operator.Add => '+',
                Operator.Subtract => '-',
                Operator.Multiply => '*',
                Operator.Divide => '/',
                Operator.Index => '^',
                _ => throw new Exception()
            };
            
            return $"({Left} {c} {Right})";
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
