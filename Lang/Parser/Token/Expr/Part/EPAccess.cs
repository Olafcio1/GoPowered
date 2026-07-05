using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPSquare(IAnyExpression? Access, IType? Type) : IExpressionPart;
}
