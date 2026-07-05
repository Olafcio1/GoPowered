using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        public virtual bool LexOperator()
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
