using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run a goroutine.
     */
    public record StmtGo(Expression Expr) : IStatement;
}
