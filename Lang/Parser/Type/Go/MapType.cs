namespace GoPowered.Lang.Parser.Type.Go
{
    public record MapType(IType Key, IType Value) : IType;
}
