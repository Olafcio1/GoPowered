using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define multiple variables with a single statement.
     */
    public record StmtMultiAssign(List<Assignment> Variables) : IStatement;
}
