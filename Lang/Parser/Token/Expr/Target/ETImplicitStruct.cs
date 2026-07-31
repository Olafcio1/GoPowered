using GoPowered.Lang.Parser.Token.AnnotatedExpr;

namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETImplicitStruct(List<IAnyExpression> Positional, Dictionary<string, AnnotatedExpression> Keyword)
                : IExpressionTarget;
}
