using System;
using Zene.Windowing;

namespace maths
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine($"{(7 + (I)8) * (4 + (I)2)}");
            Console.WriteLine($"{(1 + (I)2) ^ 6}");
            
            //Console.WriteLine($"{Exp(7 + (I)1, 5)}");
            //Console.WriteLine($"{Exp(7 + (I)1, 10)}");
            Console.WriteLine($"{Exp(7 + (I)1, 20)}");
            
            //Console.WriteLine($"{LnP1((I)0.5, 20)}");
            //Console.WriteLine($"{Ln(4 - (I)1, 20)}");
            Console.WriteLine($"{Ln((I)1, 20)}");
            Console.WriteLine($"{Ln((I)1, 40)}");*/
            
            Core.Init();
            
            //IExpression e = new Term(1d, 2d) + new Term(1d, 1d) + new Term(1d, 0d) - new Term(0.5d, 3d) - new Term(0.1d, 4d);
            IExpression e = Function.Tanh;
            //IExpression e = Function.Sin * (new Operation(Function.Cos, new Term(2, 1), Operator.Function));
            
            //Visualiser program = new Visualiser(800, 500, "ertgyh", e);
            Visualiser program = new Visualiser(800, 500, "ertgyh",
                //new Operation(Function.Sin, new Term(2, 1), Operator.Function), Function.Cos);
                //Function.Cos * new Operation(Function.Tan, new Term(3, 1), Operator.Function));
                Function.Tanh * Function.Cos, Function.Sin);
            program.Run();
            
            Core.Terminate();
        }
        
        public static Complex Exp(Complex c, int ac)
        {
            /*
            Complex v = new Complex();
            
            long r = 1;
            Complex cn = 1;
            for (int i = 0; i < ac; i++)
            {
                if (i != 0)
                {
                    r *= i;
                    cn *= c;
                }
                
                v += new Complex() { R = cn.R / r, I = (I)(cn.I.Value / r) };
            }
            
            return v;*/
            
            return (Math.Cos(c.I.Value) + (I)Math.Sin(c.I.Value)) * Math.Exp(c.R.Value);
        }
        public static Complex Sin(Complex c, int ac)
        {
            /*
            Complex v = new Complex();
            
            long r = 1;
            Complex cn = 1;
            for (int i = 0; i < ac; i++)
            {
                if (i != 0)
                {
                    r *= i;
                    cn *= c;
                }
                
                if (i % 2 == 0) { continue; }
                
                double nr = r;
                if ((i / 2) % 2 == 1) { nr = -nr; }
                
                v += new Complex() { R = cn.R / nr, I = (I)(cn.I.Value / nr) };
            }
            
            return v;*/
            return (I)(Math.Sinh(c.I.Value) * Math.Cos(c.R)) + (Math.Cosh(c.I.Value) * Math.Sin(c.R));
        }
        public static Complex Cos(Complex c, int ac)
        {
            /*
            Complex v = new Complex();
            
            long r = 1;
            Complex cn = 1;
            for (int i = 0; i < ac; i++)
            {
                if (i != 0)
                {
                    r *= i;
                    cn *= c;
                }
                
                if (i % 2 == 1) { continue; }
                
                double nr = r;
                if ((i / 2) % 2 == 1) { nr = -nr; }
                
                v += new Complex() { R = cn.R / nr, I = (I)(cn.I.Value / nr) };
            }
            
            return v;*/
            return (Math.Cosh(c.I.Value) * Math.Cos(c.R)) - (I)(Math.Sinh(c.I.Value) * Math.Sin(c.R));
        }
        public static Complex Sinh(Complex c, int ac)
        {
            return (I)(-Math.Sin(-c.I.Value) * Math.Cosh(c.R)) + (Math.Cos(-c.I.Value) * Math.Sinh(c.R));
        }
        public static Complex Cosh(Complex c, int ac)
        {
            return (Math.Cos(-c.I.Value) * Math.Cosh(c.R)) - (I)(-Math.Sin(-c.I.Value) * Math.Sinh(c.R));
        }
        public static Complex LnP1(Complex c, int ac)
        {
            Complex v = new Complex();
            
            Complex cn = 1;
            for (int i = 1; i < ac; i++)
            {
                if (i != 0) { cn *= c; }
                
                int denom = i % 2 == 0 ? -i : i;
                v += new Complex() { R = cn.R / denom, I = (I)(cn.I.Value / denom) };
            }
            
            return v;
        }
        public static Complex Ln(Complex c, int ac)
        {
            double r = c.R;
            if (r == 0)
            {
                return LnP1(c - 1, ac);
            }
            return LnP1(new Complex() { I = c.I } / r, ac) + Math.Log(r);
        }
    }
}
