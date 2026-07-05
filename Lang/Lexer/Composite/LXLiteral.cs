using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        public virtual bool LexLiteral()
        {
            var ch = Peek();
            if (!CharUtils.IsLatin(ch) && ch != '_')
                return false;

            Skip();

            var name = "" + ch;
            while (!ReachedEOF() && CharUtils.IsLiteral(Peek()))
                name += Consume();

            ILexerToken token;
            if (KeywordExtension.FromCode(name, out var value))
                token = value.ToToken();
            else token = new LTLiteral(name);

            AddToken(token);
            return true;
        }
    }
}
