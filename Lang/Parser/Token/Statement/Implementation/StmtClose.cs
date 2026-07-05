namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to close a channel.
     */
    public record StmtClose(IAnyExpression Expr) : IStatement;
}
