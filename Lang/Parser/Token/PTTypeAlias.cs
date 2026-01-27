using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token
{
    public record PTTypeAlias(
        string Name,
        IType Type
    ) : IParserToken;
}
