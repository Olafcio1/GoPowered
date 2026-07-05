using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;
using GoPowered.PoweredLang.PoweredLexer.Token;

namespace GoPowered.PoweredLang.PoweredLexer
{
    public class PoweredLexer(string input)
               : Lexer(input)
    {
        public override bool LexLiteral()
        {
            var ch = Peek();
            if (!CharUtils.IsLatin(ch) && ch != '_')
                return false;

            Skip();

            var name = "" + ch;
            while (!ReachedEOF() && CharUtils.IsLiteral(Peek()))
                name += Consume();

            ILexerToken token;
            if (VKeywordExtension.FromCode(name, out var value1))
                token = value1.ToToken();
            else if (KeywordExtension.FromCode(name, out var value))
                token = value.ToToken();
            else token = new LTLiteral(name);

            AddToken(token);
            return true;
        }
    }
}
