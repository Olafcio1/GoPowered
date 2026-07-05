namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETImplicitStruct(Dictionary<string, IAnyExpression> Fields)
                : IExpressionTarget;
}
