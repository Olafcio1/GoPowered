using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETMap(IType KeyType, IType ValueType, Dictionary<IAnyExpression, IAnyExpression> Values)
                : IExpressionTarget;
}
