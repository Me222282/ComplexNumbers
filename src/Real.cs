using System;

namespace maths
{
    public struct Real
    {
        public double Value;
        
        public override string ToString() => Value.ToString();
        
        public static implicit operator double(Real v) => v.Value;
        public static implicit operator Real(double v) => new Real() { Value = v };
        public static Real operator +(Real a, Real b) => a.Value + b.Value;
        public static Real operator -(Real a, Real b) => a.Value - b.Value;
        public static Real operator -(Real a) => -a.Value;
        public static Real operator *(Real a, Real b) => a.Value * b.Value;
        public static Real operator /(Real a, Real b) => a.Value / b.Value;
        public static Real operator ^(Real a, Real b) => Math.Pow(a.Value, b.Value);
        
        public static Complex operator +(Real a, I b) => new Complex() { R = a, I = b};
        public static Complex operator -(Real a, I b) => new Complex() { R = a, I = -b};
    }
}