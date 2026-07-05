using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseCast(List<IExpressionPart> parts)
        {
            if (Now([(null, Operator.Dot.ToToken()), (null, Operator.LParen.ToToken())], true))
            {
                parts.Add(new EPCast(ParseType()!));
                Require(Operator.RParen.ToToken(), "')'");

                return true;
            }

            return false;
        }
    }
}
