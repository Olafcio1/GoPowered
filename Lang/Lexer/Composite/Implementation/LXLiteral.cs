using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer.Composite.Implementation
{
    public sealed class LXLiteral : CLexer
    {
        internal LXLiteral(PeekT peek,
                           ConsumeCharT consumeChar,
                           ConsumeStringT consumeString,
                           SkipT skip,
                           AddTokenT addToken,
                           ReachedEOFT reachedEOF,
                           NowCharT nowChar,
                           NowStringT nowString) : base(peek, consumeChar, consumeString, skip, addToken, reachedEOF, nowChar, nowString)
        {}

        public bool LexLiteral()
        {
            var ch = Peek();
            if (!CharUtils.IsLatin(ch) && ch != '_')
                return false;

            Skip();

            var name = "" + ch;
            while (!ReachedEOF() && CharUtils.IsLiteral(Peek()))
                name += ConsumeChar();

            ILexerToken token;
            if (KeywordExtension.FromCode(name, out var value))
                token = value.ToToken();
            else token = new LTLiteral(name);

            AddToken(token);
            return true;
        }
    }
}
