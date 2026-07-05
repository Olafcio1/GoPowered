using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETNest? ParseNest(bool allowInit, bool constant)
        {
            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var expr = ParseExpression(allowInit: allowInit, constant: constant);
                Require(Operator.RParen.ToToken(), "')'");
                return new ETNest(expr);
            }

            return null;
        }
    }
}
