namespace GoPowered.Lang.Lexer.Composite
{
    public abstract class CLexer
    {
        protected internal delegate char PeekT(int index = 0);
        protected internal delegate char ConsumeCharT();
        protected internal delegate string ConsumeStringT(int length = 1);
        protected internal delegate void SkipT(int count = 1);
        protected internal delegate void AddTokenT(ILexerToken token);
        protected internal delegate bool ReachedEOFT(int further = 0);
        protected internal delegate bool NowCharT(char ch, int after = 0);
        protected internal delegate bool NowStringT(string ch, int after = 0);

        protected PeekT Peek;
        protected ConsumeCharT ConsumeChar;
        protected ConsumeStringT ConsumeString;
        protected SkipT Skip;
        protected AddTokenT AddToken;
        protected ReachedEOFT ReachedEOF;
        protected NowCharT NowChar;
        protected NowStringT NowString;

        protected internal CLexer(PeekT peek, ConsumeCharT consume, ConsumeStringT consumeString, SkipT skip, AddTokenT addToken, ReachedEOFT reachedEOF, NowCharT nowChar, NowStringT nowString)
        {
            Peek = peek;
            ConsumeChar = consume;
            ConsumeString = consumeString;
            Skip = skip;
            AddToken = addToken;
            ReachedEOF = reachedEOF;
            NowChar = nowChar;
            NowString = nowString;
        }

        protected char Consume()
        {
            return ConsumeChar();
        }

        protected string Consume(int length = 1)
        {
            return ConsumeString(length);
        }

        protected bool Now(char ch, int after = 0)
        {
            return NowChar(ch, after);
        }

        protected bool Now(string ch, int after = 0)
        {
            return NowString(ch, after);
        }
    }
}
