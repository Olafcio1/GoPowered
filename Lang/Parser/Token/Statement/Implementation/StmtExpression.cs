namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to evaluate an effective expression.
     */
    public record StmtExpression(IAnyExpression Expr) : IStatement;
}
