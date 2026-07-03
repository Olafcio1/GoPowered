using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new variables.
     */
    public record StmtAssign(string Name, Expression? Value, IType? Type) : IStatement;
}
