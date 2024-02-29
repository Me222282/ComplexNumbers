namespace maths
{
    public struct Graph
    {
        public Graph(IExpression y)
        {
            Y = y;
            X = null;
        }
        public Graph(IExpression y, IExpression x)
        {
            Y = y;
            X = x;
        }
        
        public bool Parameteric => X is not null;
        public IExpression Y { get; set; }
        public IExpression X { get; set; }
    }
}