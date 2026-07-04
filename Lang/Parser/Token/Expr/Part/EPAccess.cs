namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPAccess(IAnyExpression Member) : IExpressionPart;
}
