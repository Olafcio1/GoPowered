namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run a goroutine.
     */
    public record StmtGo(IAnyExpression Expr) : IStatement;
}
