namespace GoPowered.Lang.Parser.Token.AnnotatedExpr
{
    public record AnnotatedExpression(IAnyExpression Expression, string? Annotation);
}
