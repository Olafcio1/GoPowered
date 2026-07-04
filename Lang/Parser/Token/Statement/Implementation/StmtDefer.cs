namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to defer a function call to the return time.
     */
    public record StmtDefer(IAnyExpression Expr) : IStatement;
}
