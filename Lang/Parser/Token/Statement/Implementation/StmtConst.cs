using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to define new constants.
     */
    public record StmtConst(string Name, IType Type, IAnyExpression Value) : IStatement;
}
