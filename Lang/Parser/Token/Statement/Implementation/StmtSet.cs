using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to set a field (of an object) or element (typically of a map or array).
     */
    public record StmtSet(Expression Name, Expression Value) : IStatement;
}
