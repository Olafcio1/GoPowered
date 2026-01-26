using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token
{
    public record PTFunction(
        string Name,
        List<Argument> Arguments,
        List<IStatement> Body,
        List<ReturnValue>? Returns
    ) : IParserToken;

    public record Argument(
        string Name,
        IType Type
    );

    public record ReturnValue(
        string? Name,
        IType Type
    );
}
