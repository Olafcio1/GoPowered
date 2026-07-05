using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool Assigning()
        {
            var i = 0;

            while (!ReachedEOF(i + 2))
            {
                if (Peek(i++) is not LTLiteral)
                    break;

                if (Peek(i++) is not LTOperator op)
                    break;

                if (op.Value == Operator.Comma)
                    continue;
                else if (op.Value == Operator.Assign)
                    return true;
                else
                    break;
            }

            return false;
        }

        private partial bool Setting(int i = 0)
        {
            while (!ReachedEOF(i + 2))
            {
                if (Peek(i++) is not LTLiteral)
                    break;

                if (Peek(i++) is not LTOperator op)
                    break;

                if (op.Value == Operator.Comma)
                    continue;
                else if (op.Value == Operator.Set)
                    return true;
                else
                    break;
            }

            return false;
        }
    }
}
