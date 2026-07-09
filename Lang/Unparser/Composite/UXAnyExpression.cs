using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleAnyExpression(IAnyExpression anyExpr)
        {
            if (anyExpr is Expression expr)
                return HandleExpression(expr);
            else
                throw new UnparserError("Expected an expression");
        }
    }
}
