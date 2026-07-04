namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPSlice(IAnyExpression? From, IAnyExpression? To) : IExpressionPart;
}
