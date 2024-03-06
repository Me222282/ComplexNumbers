using System;
using System.Collections.Generic;
using Zene.Structs;

namespace maths
{
    public struct Complex
    {
        private const double _pi2 = Math.PI * 2d;
        
        public Real R;
        public I I;
        
        public double Modulus() => Math.Sqrt((R.Value * R.Value) + (I.Value * I.Value));
        public double Argument()
        {
            double arg = Math.Atan(I.Value / R.Value);
            if (R.Value < 0)
            {
                arg += Math.PI;
            }
            if (arg > Math.PI)
            {
                arg -= _pi2;
            }
            return arg;
        }
        
        public override string ToString()
        {
            if (I.Value == 0d)
            {
                return R.ToString();
            }
            if (R.Value == 0d)
            {
                return I.ToString();
            }
            
            return $"{R} + {I}";
        }
        
        public override bool Equals(object obj)
        {
            return obj is Complex complex &&
                R == complex.R &&
                I == complex.I;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, I);
        }
        
        public static bool operator ==(Complex l, Complex r) => l.Equals(r);
        public static bool operator !=(Complex l, Complex r) => !l.Equals(r);
        
        public static explicit operator Complex(Vector2 v) => new Complex() { R = v.X, I = (I)v.Y };
        public static implicit operator Complex(double v) => new Complex() { R = v };
        public static implicit operator Complex(I v) => new Complex() { I = v };
        public static Complex operator +(Complex a, Complex b)
            => new Complex() { R = a.R + b.R, I = a.I + b.I };
        public static Complex operator -(Complex a, Complex b)
            => new Complex() { R = a.R - b.R, I = a.I - b.I };
        public static Complex operator -(Complex a)
            => new Complex() { R = -a.R, I = -a.I };
        public static Complex operator *(Complex a, Complex b)
            => new Complex()
            {
                R = (a.R * b.R) - (a.I.Value * b.I.Value),
                I = (I)(a.R.Value * b.I.Value) + (I)(a.I.Value * b.R.Value)
            };
        public static Complex operator /(Complex a, Complex b)
        {
            double denom = (b.R.Value * b.R.Value) + (b.I.Value * b.I.Value);
            
            return new Complex()
            {
                R = ((a.R * b.R) + (a.I.Value * b.I.Value)) / denom,
                I = (I)(((a.R.Value * b.I.Value) - (a.I.Value * b.R.Value)) / denom)
            };
        }
        public static Complex operator ^(Complex a, int i)
        {
            if (i == 0) { return 1d;}
            if (i == 1) { return a; }
            if (i == -1) { return 1d / a; }
            
            bool recp = false;
            if (i < 0)
            {
                recp = true;
                i = -i;
            }
            Complex r = new Complex();
            
            for (int j = 0; j <= i; j++)
            {
                double v = nCr(i, j) * Math.Pow(a.R.Value, i - j) * Math.Pow(a.I.Value, j);
                
                if ((j / 2) % 2 == 1) { v = -v; }
                if (j % 2 == 1)
                {
                    r.I.Value += v;
                    continue;
                }
                
                r.R.Value += v;
            }
            
            if (recp) { return 1d / r; }
            return r;
        }
        public static Complex operator ^(Complex a, Complex b)
        {
            if (a.I.Value == 0d)
            {
                if (b.I.Value == 0d)
                {
                    return Math.Pow(a.R, b.R);
                }
                
                return Program.Exp(Math.Log(a.R) * b, 20);
            }
            
            if (b.I.Value == 0d && ((int)b.R) == b.R)
            {
                return a ^ (int)b.R;
            }
            
            return Program.Exp(Program.Ln(a, 20) * b, 20);
        }
        
        public static long nCr(int n, int r)
        {
            // naive: return Factorial(n) / (Factorial(r) * Factorial(n - r));
            return nPr(n, r) / Factorial(r);
        }
        public static long nPr(int n, int r)
        {
            // naive: return Factorial(n) / Factorial(n - r);
            return FactorialDivision(n, n - r);
        }
        private static long FactorialDivision(int topFactorial, int divisorFactorial)
        {
            long result = 1;
            for (int i = topFactorial; i > divisorFactorial; i--)
            {
                result *= i;
            }
            return result;
        }
        private static long Factorial(int i)
        {
            if (i <= 1) { return 1; }
            return i * Factorial(i - 1);
        }
    }
}