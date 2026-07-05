using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETSlice(IType ElementType, List<IAnyExpression> Values)
                : IExpressionTarget;
}
