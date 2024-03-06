namespace maths
{
    public interface IWordParser
    {
        public void Reset();
        public bool TryParse(Keyword word);
        
        public IExpression Expression { get; }
        public Operator Op { get; }
        public int Bracket { get; }
    }
}