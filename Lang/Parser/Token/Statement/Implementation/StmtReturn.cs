namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to return values from a function.
     */
    public record StmtReturn(List<IAnyExpression>? Values) : IStatement;
}
