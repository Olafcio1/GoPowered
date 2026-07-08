using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        private static readonly Dictionary<char, string> str_special = new()
        {
            ['n'] = "\n",
            ['r'] = "\r",
            ['t'] = "\t",
            ['b'] = "\b",
            ['e'] = "\e",
            ['a'] = "\a",
            ['f'] = "\f"
        };

        public virtual bool LexString()
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
                    else if (str_special.ContainsKey(nxt))
                        value += str_special.GetValueOrDefault(nxt);
                    else throw new LexerError("unrecognized \\" + nxt);
                } else if (Now('\n'))
                {
                    throw new LexerError("non-multiline strings cannot contain newlines");
                } else
                {
                    value += Consume();
                }
            }

            throw new LexerError("unterminated string");
        }
    }
}
