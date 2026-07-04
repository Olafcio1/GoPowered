namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new constants.
     */
    public record StmtConst(string Name, IAnyExpression Value) : IStatement;
}
