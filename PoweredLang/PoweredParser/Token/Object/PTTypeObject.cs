using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.PoweredLang.PoweredParser.Token.Object
{
    /**
     * A struct that can only be initialized within the package.
     */
    public record PTTypeObject(
        string Name,
        Dictionary<string, IType> Fields,
        List<string> Inherits,
        Dictionary<string, IType>? Generics
    ) : IParserToken;
}
