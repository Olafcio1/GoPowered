namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETConvert(string Name, IAnyExpression Expr) : IExpressionTarget;
}
