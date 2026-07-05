using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Object
{
    public record PTTypeInterface(
        string Name,
        Dictionary<string, FunctionSignature> Methods,
        List<string> Inherits,
        Dictionary<string, IType>? Generics
    ) : IParserToken;
}
