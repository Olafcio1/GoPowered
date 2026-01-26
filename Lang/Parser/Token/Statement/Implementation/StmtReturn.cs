using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to return values from a function.
     */
    public record StmtReturn(List<Expression> Values) : IStatement;
}
