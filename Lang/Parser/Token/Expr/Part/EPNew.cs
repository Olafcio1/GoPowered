using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Part
{
    public record EPNew(
        List<Expression> Positional,
        Dictionary<string, Expression> Keyword,
        List<IType>? Generics
    ) : IExpressionPart;
}
