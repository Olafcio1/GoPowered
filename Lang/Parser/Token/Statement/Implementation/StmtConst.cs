using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new constants.
     */
    public record StmtConst(string Name, IExpressionTarget Value) : IStatement;
}
