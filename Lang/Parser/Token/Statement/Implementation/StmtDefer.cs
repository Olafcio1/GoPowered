using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to defer a function call to the return time.
     */
    public record StmtDefer(Expression Expr) : IStatement;
}
