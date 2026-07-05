namespace GoPowered.Lang.Parser.Type.Go
{
    public record AnyType : IType, IAvoidSerialization
    {
        public static readonly AnyType INSTANCE = new();

        internal AnyType() {}

        public override string ToString()
        {
            return "";
        }
    }
}
