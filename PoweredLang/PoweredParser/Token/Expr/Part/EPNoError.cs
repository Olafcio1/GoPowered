using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.PoweredLang.PoweredParser.Token.Expr.Part
{
    public record EPNoError(int Errors) : IExpressionPart;
}
