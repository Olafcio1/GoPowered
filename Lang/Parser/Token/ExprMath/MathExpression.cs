using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.ExprMath
{
    public record MathExpression(
        IAnyExpression Target,
        List<MathMember> Members
    ) : IAnyExpression;
}
