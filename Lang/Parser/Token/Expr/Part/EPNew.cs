using GoPowered.Lang.Parser.Token.AnnotatedExpr;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPNew(
        List<IAnyExpression> Positional,
        Dictionary<string, AnnotatedExpression> Keyword
    ) : IExpressionPart;
}
