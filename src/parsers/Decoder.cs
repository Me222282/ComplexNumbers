using System;
using System.Collections.Generic;
using System.Text;

namespace maths
{
    public class Decoder
    {
        public Decoder()
        {
            
        }
        
        public readonly List<IWordParser> Parsers = new List<IWordParser>()
        {
            new NumberParser(), new XParser(), new BracketParser(),
            new OperatorParser(), new FuncParser(), new ConstParser()
        };
        
        private DecodeInst _inst;
        public IExpression Expression => _inst.Current;
        
        public void Decode(string str)
        {
            Span<Keyword> sk = Getwords(str);
            
            _inst = new DecodeInst();
            for (int i = 0; i < sk.Length; i++)
            {
                Manage(sk[i]);
            }
        }
        private void Manage(Keyword word)
        {
            IWordParser wp = null;
            
            foreach (IWordParser p in Parsers)
            {
                p.Reset();
                if (!p.TryParse(word)) { continue; }
                
                wp = p;
                break;
            }
            
            if (wp is null)
            {
                throw new Exception();
            }
            
            _inst.Manage(wp);
        }
        private Span<Keyword> Getwords(string str)
        {
            str = str.Trim();

            List<Keyword> keywords = new List<Keyword>(str.Length);

            int bracket = 0;
            int bracketSquare = 0;
            int bracketCurle = 0;

            bool newWord = true;
            bool inNumber = false;
            bool inText = false;

            KeywordType type = 0;

            StringBuilder word = new StringBuilder();
            
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == ')')
                {
                    newWord = true;
                    inNumber = false;
                    inText = false;

                    if (word.Length > 0)
                    {
                        keywords.Add(new Keyword(word.ToString(), type));
                        word.Clear();
                    }

                    bracket--;
                    keywords.Add(new Keyword(")", KeywordType.BracketClosed, bracket));
                    continue;
                }
                if (c == ']')
                {
                    newWord = true;
                    inNumber = false;
                    inText = false;

                    if (word.Length > 0)
                    {
                        keywords.Add(new Keyword(word.ToString(), type));
                        word.Clear();
                    }

                    bracketSquare--;
                    keywords.Add(new Keyword("]", KeywordType.BracketClosed, bracketSquare));
                    continue;
                }
                if (c == '}')
                {
                    newWord = true;
                    inNumber = false;
                    inText = false;

                    if (word.Length > 0)
                    {
                        keywords.Add(new Keyword(word.ToString(), type));
                        word.Clear();
                    }

                    bracketCurle--;
                    keywords.Add(new Keyword("}", KeywordType.BracketClosed, bracketCurle));
                    continue;
                }

                if (newWord)
                {
                    if (c == '(')
                    {
                        keywords.Add(new Keyword("(", KeywordType.BracketOpen, bracket));
                        bracket++;
                        continue;
                    }
                    if (c == '[')
                    {
                        keywords.Add(new Keyword("[", KeywordType.BracketOpen, bracketSquare));
                        bracketSquare++;
                        continue;
                    }
                    if (c == '{')
                    {
                        keywords.Add(new Keyword("{", KeywordType.BracketOpen, bracketCurle));
                        bracketCurle++;
                        continue;
                    }
                    if (char.IsNumber(c))
                    {
                        newWord = false;
                        inNumber = true;
                        type = KeywordType.Number;

                        word.Append(c);

                        continue;
                    }
                    if (c == '_' || char.IsLetter(c))
                    {
                        newWord = false;
                        inText = true;
                        type = KeywordType.String;

                        word.Append(c);

                        continue;
                    }
                    if (!char.IsWhiteSpace(c))
                    {
                        keywords.Add(new Keyword(c.ToString(), KeywordType.Special));

                        continue;
                    }

                    continue;
                }
                
                // Underscores can be insinde numbers
                if (inNumber && c == '_' &&
                    // not at end of string
                    str.Length > (i + 10) &&
                    char.IsNumber(str[i + 1]))
                {
                    continue;
                }
                if (inNumber && c != '.' && !char.IsNumber(c))
                {
                    newWord = true;
                    inNumber = false;

                    keywords.Add(new Keyword(word.ToString(), KeywordType.Number));
                    word.Clear();

                    i--;
                    continue;
                }

                if (inText && c != '_' && !char.IsNumber(c) && !char.IsLetter(c))
                {
                    newWord = true;
                    inText = false;

                    keywords.Add(new Keyword(word.ToString(), KeywordType.String));
                    word.Clear();

                    i--;
                    continue;
                }

                word.Append(c);
            }

            if (word.Length > 0)
            {
                keywords.Add(new Keyword(word.ToString(), type));
            }

            return keywords.ToArray();
        }
        
        private class DecodeInst
        {
            public DecodeInst() { }
            private DecodeInst(int i)
            {
                Id = i;
            }
            
            private DecodeInst _inst;
            public Operation Op;
            public IExpression Current;
            public int Id;
            public bool Subsec;
            public bool Subsec2;
            
            public bool Negate;
            
            public void Manage(IWordParser wp)
            {                
                if (_inst is not null)
                {
                    if (wp.Bracket != _inst.Id)
                    {
                        _inst.Manage(wp);
                        return;
                    }
                    
                    if (Op is not null)
                    {
                        Op.Right = _inst.Current;
                        if (Current is null)
                        {
                            Current = Op;
                        }
                        else
                        {
                            Subsec = false;
                        }
                        Op = null;
                    }
                    else if (Current is not null)
                    {
                        Current = Mult(Current, _inst.Current);
                    }
                    else
                    {
                        Current = _inst.Current;
                    }
                    _inst = null;
                    return;
                }
                
                if (wp.Bracket >= 0)
                {
                    _inst = new DecodeInst(wp.Bracket);
                    Subsec = true;
                    Subsec2 = true;
                    return;
                }
                
                M2(wp);
                Subsec = false;
                Subsec2 = false;
            }
            private void M2(IWordParser wp)
            {
                IExpression e = wp.Expression;
                Operator o = wp.Op;
                
                if (Negate)
                {
                    e = e.Negative();
                    Negate = false;
                }
                
                if (e is null)
                {
                    if (Current is null || Op is not null)
                    {
                        if (o == Operator.Subtract)
                        {
                            Negate = true;
                            return;
                        }
                        throw new Exception();
                    }
                    if (!Subsec && Current is Operation op)
                    {
                        if (!Subsec2 && op.Right is Operation opr &&
                            (o == Operator.Index) &&
                            (opr.Operator == Operator.Multiply || opr.Operator == Operator.Divide))
                        {
                            Op = new Operation(opr.Right, null, o);
                            opr.Right = Op;
                            return;
                        }
                        
                        if (((o != Operator.Add && o != Operator.Subtract) &&
                            (op.Operator == Operator.Add || op.Operator == Operator.Subtract)) ||
                            ((o == Operator.Index) &&
                            (op.Operator == Operator.Multiply || op.Operator == Operator.Divide)))
                        {
                            Op = new Operation(op.Right, null, o);
                            op.Right = Op;
                            return;
                        }
                    }
                    Op = new Operation(Current, null, o);
                    Current = null;
                    return;
                }
                
                if (o == Operator.Function)
                {
                    Operation n = new Operation(e, null, Operator.Function);
                    if (Op is not null)
                    {
                        Op.Right = n;
                        Current = Op;
                    }
                    else if (Current is not null)
                    {
                        Current = Mult(Current, n);
                    }
                    Op = n;
                    return;
                }
                
                if (Op is null)
                {   
                    if (Current is not null)
                    {
                        Current = Mult(Current, e);
                        return;
                    }
                    Current = e;
                    return;
                }
                
                Op.Right = e;
                if (Current is null)
                {
                    Current = Op;
                }
                Op = null;
            }
            private IExpression Mult(IExpression l, IExpression r)
            {
                if (l is not Operation op)
                {
                    return l * r;
                }
                
                if (op.Operator != Operator.Add && op.Operator != Operator.Subtract)
                {
                    return l * r;
                }
                
                op.Right = Mult(op.Right, r);
                Subsec = false;
                return op;
            }
        }
    }
}