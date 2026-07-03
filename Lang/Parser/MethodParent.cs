using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public record MethodParent(
        string? AssignName,
        IType Type
    );
}
