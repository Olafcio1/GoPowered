namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETImplicitStruct(List<IAnyExpression> Positional, Dictionary<string, IAnyExpression> Keyword)
                : IExpressionTarget;
}
