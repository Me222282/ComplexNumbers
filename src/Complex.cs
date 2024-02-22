using System;

namespace maths
{
    public struct Complex
    {
        public Real R;
        public I I;
        
        public override string ToString() => $"{R} + {I}";
        
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
            Complex r = new Complex();
            
            for (int j = 0; j <= i; j++)
            {
                double v = nCr(i, j) * Math.Pow(a.R.Value, i - j) - Math.Pow(a.I.Value, j);
                
                if ((j / 2) % 2 == 1) { v = -v; }
                if (j % 2 == 1)
                {
                    r.I.Value += v;
                    continue;
                }
                
                r.R.Value += v;
            }
            
            return r;
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