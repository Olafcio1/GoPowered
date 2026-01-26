using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer.Composite.Implementation
{
    public sealed class LXString : CLexer
    {
        private static readonly Dictionary<char, string> special = [];
        static LXString() {
            special.Add('n', "\n");
            special.Add('r', "\r");
            special.Add('t', "\t");
            special.Add('b', "\b");
            special.Add('e', "\e");
            special.Add('a', "\a");
            special.Add('f', "\f");
        }

        internal LXString(PeekT peek,
                          ConsumeCharT consumeChar,
                          ConsumeStringT consumeString,
                          SkipT skip,
                          AddTokenT addToken,
                          ReachedEOFT reachedEOF,
                          NowCharT nowChar,
                          NowStringT nowString) : base(peek, consumeChar, consumeString, skip, addToken, reachedEOF, nowChar, nowString)
        {}

        public bool LexString()
        {
            if (!Now('"'))
                return false;

            var value = "";
            while (true)
            {
                if (Now('"'))
                {
                    AddToken(new LTString(value));
                    return true;
                } else if (Now('\\'))
                {
                    var nxt = Consume();
                    if (nxt == 'u')
                        value += char.ConvertFromUtf32(int.Parse(Consume(4)));
                    else if (nxt == 'x')
                        value += (char)int.Parse(Consume(2));
                    else if (nxt == '"')
                        value += nxt;
                    else if (special.ContainsKey(nxt))
                        value += special.GetValueOrDefault(nxt);
                    else throw new LexerError("unrecognized \\" + nxt);
                } else
                {
                    value += Consume();
                }
            }

            throw new LexerError("unterminated string");
        }
    }
}
