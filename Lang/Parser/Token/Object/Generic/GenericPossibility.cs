using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Object.Generic
{
    public record GenericPossibility(IType Type, bool allowedInheritors);
}
