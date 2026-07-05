using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETReceive? ParseReceive(bool allowInit, bool constant)
        {
            if (Now([(null, Operator.Transmit.ToToken())], true))
            {
                var expr = ParseExpression(allowInit: allowInit, constant: constant);
                return new ETReceive(expr);
            }

            return null;
        }
    }
}
