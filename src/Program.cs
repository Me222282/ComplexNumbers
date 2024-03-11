using System;
using Zene.Windowing;

namespace maths
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Init();
            
            Decoder decode = new Decoder();
            
            while (true)
            {
                Graph graph = new Graph();
                Console.Write("Q, S, P or 3: ");
                ConsoleKeyInfo cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.Q) { break; }
                if (cki.Key == ConsoleKey.S)
                {
                    Console.WriteLine();
                    Console.WriteLine("Equation:");
                    string xy = Console.ReadLine();
                    try
                    {
                        decode.Decode(xy);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.Y = decode.Expression.Simplify();
                    
                    Console.WriteLine(graph.Y);
                }
                else if (cki.Key == ConsoleKey.P)
                {
                    Console.WriteLine();
                    Console.WriteLine("Equation for X:");
                    string tx = Console.ReadLine();
                    try
                    {
                        decode.Decode(tx);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.X = decode.Expression.Simplify();
                    Console.WriteLine("Equation for Y:");
                    string ty = Console.ReadLine();
                    try
                    {
                        decode.Decode(ty);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.Y = decode.Expression.Simplify();
                    
                    Console.WriteLine(graph.X);
                    Console.WriteLine(graph.Y);
                }
                else if (cki.Key == ConsoleKey.D3)
                {
                    Console.WriteLine();
                    Console.WriteLine("Equation for X:");
                    string tx = Console.ReadLine();
                    try
                    {
                        decode.Decode(tx);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.X = decode.Expression.Simplify();
                    Console.WriteLine("Equation for Y:");
                    string ty = Console.ReadLine();
                    try
                    {
                        decode.Decode(ty);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.Y = decode.Expression.Simplify();
                    Console.WriteLine("Equation for Z:");
                    string tz = Console.ReadLine();
                    try
                    {
                        decode.Decode(tz);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occurred.");
                        continue;
                    }
                    graph.Z = decode.Expression.Simplify();
                    
                    Console.WriteLine(graph.X);
                    Console.WriteLine(graph.Y);
                    Console.WriteLine(graph.Z);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid command.");
                    continue;
                }
                
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                
                Visualiser program = new Visualiser(800, 500, "ertgyh", graph);
                program.Run();
                program.Dispose();
            }
            /*
            //Visualiser program = new Visualiser(800, 500, "ertgyh", e);
            Visualiser program = new Visualiser(800, 500, "ertgyh",
                //Function.Sin < new Term(2, 1), Function.Cos);
                //Function.Cos * Function.Tan < new Term(3, 1));
                Function.Tanh * Function.Cos, Function.Sin);
                //new Term(1d, 1d) * Function.Sech, new Term(1d, 1d) * Function.Cosech);
            program.Run();*/
            
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
        public static Complex Ln(Complex c, int ac) => Math.Log(c.Modulus()) + (I)c.Argument();
    
        public static Complex Arctan(Complex c)
        {
            double r = 0.5 * Math.Atan((2 * c.R.Value) / (1 - (c.R.Value * c.R.Value) - (c.I.Value * c.I.Value)));
            double i = -0.5 * Math.Atanh((-2 * c.I.Value) / (1 + (c.R.Value * c.R.Value) + (c.I.Value * c.I.Value)));
            if (c.R.Value > 0 && r < 0)
            {
                r += Math.PI / 2d;
            }
            else if (c.R.Value < 0 && r > 0)
            {
                r -= Math.PI / 2d;
            }
            
            return r + (I)i;
        }
    }
}
