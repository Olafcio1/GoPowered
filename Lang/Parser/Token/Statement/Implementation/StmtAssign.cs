using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new variables.
     */
    public record StmtAssign(string Name, IAnyExpression? Value, IType? Type)
                : IAssignment, IStatement;
}
