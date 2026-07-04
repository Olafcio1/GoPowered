using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPNew(
        List<IAnyExpression> Positional,
        Dictionary<string, IAnyExpression> Keyword,
        List<IType>? Generics
    ) : IExpressionPart;
}
