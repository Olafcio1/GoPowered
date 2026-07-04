namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPCall(List<IAnyExpression> Arguments) : IExpressionPart;
}
