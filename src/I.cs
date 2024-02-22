using System;

namespace maths
{
    public struct I
    {
        public double Value;
        
        public override string ToString() => $"{Value}i";
        
        public static explicit operator double(I v) => v.Value;
        public static explicit operator I(double v) => new I() { Value = v };
        public static I operator +(I a, I b) => (I)(a.Value + b.Value);
        public static I operator -(I a, I b) => (I)(a.Value - b.Value);
        public static I operator -(I a) => (I)(-a.Value);
        public static Real operator *(I a, I b) => a.Value * b.Value;
        public static Real operator /(I a, I b) => a.Value / b.Value;
        //public static I operator ^(I a, I b) => Math.Pow(a._value, b._value);
    }
}