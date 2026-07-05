using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        private static readonly Dictionary<char, char> char_special = new()
        {
            ['n'] = '\n',
            ['r'] = '\r',
            ['t'] = '\t',
            ['b'] = '\b',
            ['e'] = '\e',
            ['a'] = '\a',
            ['f'] = '\f'
        };

        public virtual bool LexChar()
        {
            if (!Now("'"))
                return false;

            char value;

            if (Now('\\'))
            {
                var nxt = Consume();
                if (nxt == 'u')
                    value = (char) int.Parse(Consume(4));
                else if (nxt == 'x')
                    value = (char) int.Parse(Consume(2));
                else if (nxt == '"')
                    value = nxt;
                else if (char_special.ContainsKey(nxt))
                    value = char_special.GetValueOrDefault(nxt);
                else throw new LexerError("unrecognized \\" + nxt);
            } else
            {
                value = Consume();
            }

            if (!Now("'"))
                throw new LexerError("expected '");

            AddToken(new LTChar(value));

            return true;
        }
    }
}
