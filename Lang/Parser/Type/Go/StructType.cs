namespace GoPowered.Lang.Parser.Type.Go
{
    public record StructType(Dictionary<string, IType> Fields, List<string> Inherits)
                : IType;
}
