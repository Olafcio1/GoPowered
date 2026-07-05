using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to extract an expression into new assigned variables.
     */
    public record StmtExtractAssign(List<string?> Names, IAnyExpression? Value, IType? Type)
                : IStatement;
}
