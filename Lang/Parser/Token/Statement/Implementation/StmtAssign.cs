using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new variables.
     */
    public record StmtAssign(string Name, Expression Value) : IStatement;
}
