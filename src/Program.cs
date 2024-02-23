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
            
            Visualiser program = new Visualiser(800, 500, "ertgyh", new Term(3d, 0d));
            program.Run();
            
            Core.Terminate();
        }
        
        public static Complex Exp(Complex c, int ac)
        {
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
            
            return v;
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
