using GoPowered.Lang.Parser.Token.Base;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token
{
    public record PTFunction(
        string Name,
        List<Argument> Arguments,
        List<IStatement> Body
    ) : IParserToken;

    public record Argument(
        string Name,
        IType Type
    );
}
