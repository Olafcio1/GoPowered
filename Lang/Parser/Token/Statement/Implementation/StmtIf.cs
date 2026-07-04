using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.Statement.Implementation.If;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to perform code based on conditions.
     */
    public record StmtIf(List<Branch> Branches, List<IStatement>? Else) : IStatement;
}
