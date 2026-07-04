using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Assign
{
    public record Assignment(string Name, IAnyExpression? Value, IType? Type)
                : IAssignment;
}
