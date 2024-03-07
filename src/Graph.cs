using System;
using Zene.Structs;

namespace maths
{
    public struct Graph
    {
        public Graph(IExpression y)
        {
            Y = y;
            X = null;
            Z = null;
        }
        public Graph(IExpression y, IExpression x)
        {
            Y = y;
            X = x;
            Z = null;
        }
        public Graph(IExpression y, IExpression x, IExpression z)
        {
            Y = y;
            X = x;
            Z = z;
        }
        
        public bool Parameteric => X is not null;
        public bool ThreeQ => Z is not null;
        public IExpression Y { get; set; }
        public IExpression X { get; set; }
        
        public IExpression Z { get; set; }
        
        public Vector3 Calculate(Complex c, Mode m)
        {
            Complex comp = Y.Calculate(c);
            double d = GetReal(comp, m);
            
            if (ThreeQ)
            {
                Complex x = X.Calculate(c);
                Complex z = Z.Calculate(c);
                return (GetReal(x, m), d, GetReal(z, m));
            }
            
            if (Parameteric)
            {
                Complex vn = X.Calculate(c);
                return (vn.R.Value, d, vn.I.Value);
            }
            else
            {
                return (c.R.Value, d, c.I.Value);
            }
        }
        private double GetReal(Complex c, Mode m)
        {
            return m switch
            {
                Mode.Mag => c.Modulus(),
                Mode.Real => c.R,
                Mode.I => c.I.Value,
                Mode.Sum => c.R + c.I.Value,
                Mode.Arg => c.Argument(),
                _ => throw new Exception()
            };
        }
    }
}