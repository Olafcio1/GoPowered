using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        public virtual bool LexMulString()
        {
            if (!Now('`'))
                return false;

            var value = "";
            while (true)
            {
                if (Now('`'))
                {
                    AddToken(new LTString(value));
                    return true;
                } else
                {
                    value += Consume();
                }
            }

            throw new LexerError("unterminated multiline string");
        }
    }
}
