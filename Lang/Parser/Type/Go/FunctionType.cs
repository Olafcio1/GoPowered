using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Parser.Type.Go
{
    public record FunctionType(
        List<Argument> Args,
        List<ReturnValue>? Returns,
        Dictionary<string, IType>? Generics
    ) : IType;
}
