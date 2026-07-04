using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Object
{
    public record FunctionSignature(
        string Name,
        List<Argument> Args,
        List<ReturnValue>? Returns,
        Dictionary<string, IType>? Generics
    ) : IParserToken;
}
