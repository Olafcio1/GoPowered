namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPNew(Dictionary<string, Expression> Fields) : IExpressionPart;
}
