using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Assign
{
    public interface IAssignment
    {
        public string? Name { get; }
        public IAnyExpression? Value { get; }
        public IType? Type { get; }
    }
}
