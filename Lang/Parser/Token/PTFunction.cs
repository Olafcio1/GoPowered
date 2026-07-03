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

    public class Argument(
        string Name,
        IType Type
    )
    {
        public string Name = Name;
        public IType Type = Type;
    }

    public record ReturnValue(
        string? Name,
        IType Type
    );
}
