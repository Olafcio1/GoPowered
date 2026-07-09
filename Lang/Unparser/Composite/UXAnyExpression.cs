using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.ExprMath;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleAnyExpression(IAnyExpression anyExpr)
        {
            if (anyExpr is Expression expr)
                return HandleExpression(expr);
            else if (anyExpr is MathExpression mathExpr)
                return HandleMathExpression(mathExpr);
            else
                throw new UnparserError("Expected an expression");
        }
    }
}
