using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETMake(IType Type, List<IAnyExpression> Values) : IExpressionTarget;
}
