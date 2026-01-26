using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer.Composite.Implementation
{
    public sealed class LXOperator : CLexer
    {
        internal LXOperator(PeekT peek,
                            ConsumeCharT consumeChar,
                            ConsumeStringT consumeString,
                            SkipT skip,
                            AddTokenT addToken,
                            ReachedEOFT reachedEOF,
                            NowCharT nowChar,
                            NowStringT nowString) : base(peek, consumeChar, consumeString, skip, addToken, reachedEOF, nowChar, nowString)
        {}

        public bool LexOperator()
        {
            foreach (var op in Operator.Values())
            {
                var str = op.ToCode()!;

                var ok = true;
                var i = 0;

                foreach (var ch in str)
                {
                    if (Peek(i++) != ch)
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    Skip(i);
                    AddToken(new LTOperator(op));

                    return true;
                }
            }

            return false;
        }
    }
}
