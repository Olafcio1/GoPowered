using GoPowered.Lang.Parser.Token.Object;

namespace GoPowered.Lang.Parser.Type.Go
{
    public record InterfaceType(Dictionary<string, FunctionSignature> Methods, List<string> Inherits)
                : IType;
}
