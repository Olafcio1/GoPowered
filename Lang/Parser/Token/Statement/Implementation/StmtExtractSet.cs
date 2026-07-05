using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to extract an expression into already existing variables.
     */
    public record StmtExtractSet(List<IAnyExpression> ExtractFrom, List<Expression> ExtractTo)
                : IStatement;
}
