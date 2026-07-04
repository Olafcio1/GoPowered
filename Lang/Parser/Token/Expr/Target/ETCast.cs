namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETCast(string Name, IAnyExpression Expr) : IExpressionTarget;
}
