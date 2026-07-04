using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.ExprMath
{
    public record MathExpression(
        Expression Target,
        List<MathMember> Members
    ) : IAnyExpression;
}
