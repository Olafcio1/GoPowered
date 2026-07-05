using GoPowered.Lang.Parser.Token.Object.Section;

namespace GoPowered.Lang.Parser.Type.Go
{
    public record InterfaceType(Dictionary<string, FunctionSignature> Methods, List<string> Inherits)
                : IType;
}
