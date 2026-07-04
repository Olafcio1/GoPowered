using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to iterate through an iterator.
     */
    public record StmtForRange(List<string?>? Variables, Expression Iterator, List<IStatement> Effect)
                : IStatement;
}
