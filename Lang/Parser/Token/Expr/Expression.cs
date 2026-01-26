namespace GoPowered.Lang.Parser.Token.Expr
{
    public record Expression(
        IExpressionTarget Target,
        List<IExpressionPart>? Parts,
        bool Singular = false
    );
}
