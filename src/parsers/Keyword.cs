namespace maths
{
    public enum KeywordType
    {
        BracketOpen = 0b_01,
        BracketClosed = 0b_10,
        Bracket = BracketOpen | BracketClosed,
        Number,
        String,
        Special
    }
    
    public readonly struct Keyword
    {
        public Keyword(string key, KeywordType type, int b = -1)
        {
            Word = key;
            Type = type;
            Bracket = b;
        }
        
        public string Word { get; }
        public KeywordType Type { get; }
        public int Bracket { get; }
    }
}