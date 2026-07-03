using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPNew(Dictionary<string, Expression> Fields, List<IType>? Generics) : IExpressionPart;
}
