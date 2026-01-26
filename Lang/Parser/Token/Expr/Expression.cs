namespace GoPowered.Lang.Parser.Token.Expr
{
    public record Expression(
        IExpressionTarget Target,
        List<IExpressionPart>? Parts,
        int Pointers,
        bool Singular = false
    );
}
