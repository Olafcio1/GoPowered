using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.ExprLogic;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * A C-style for loop.
     */
    public record StmtForLoop(IStatement? Initial, Condition Condition, IStatement? After, List<IStatement> IterationEffect)
                : IStatement;
}
