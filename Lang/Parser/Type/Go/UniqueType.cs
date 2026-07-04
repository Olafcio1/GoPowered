namespace GoPowered.Lang.Parser.Type.Go
{
    public record UniqueType(
        string Name,
        List<IType>? Generics
    ) : IType;
}
