using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Object
{
    public record PTTypeStruct(
        string Name,
        Dictionary<string, IType> Fields,
        List<string> Inherits
    ) : IParserToken;
}
