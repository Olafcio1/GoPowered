using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPCast(IType Type) : IExpressionPart;
}
