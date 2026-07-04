using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token
{
    public record PTTypeClone(
        string Name,
        IType Type,
        Dictionary<string, IType>? Generics
    ) : IParserToken;
}
