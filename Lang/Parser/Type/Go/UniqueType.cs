namespace GoPowered.Lang.Parser.Type.Go
{
    public record UniqueType(
        List<string> Location,
        List<IType>? Generics
    ) : IType;
}
