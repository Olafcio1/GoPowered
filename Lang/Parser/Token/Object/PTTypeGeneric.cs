using GoPowered.Lang.Parser.Token.Object.Generic;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Object
{
    public record PTTypeGeneric(
        string Name,
        List<GenericPossibility> Types,
        Dictionary<string, IType>? Generics
    ) : IParserToken;
}
