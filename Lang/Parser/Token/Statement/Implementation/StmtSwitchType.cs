using GoPowered.Lang.Parser.Token.Statement.Implementation.Switch;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run code for the first case that the value of matches the target expression's type.
     */
    public class StmtSwitchType : IStatement
    {
        public required IAnyExpression Value;
        public required List<SwitchTypeCase> Cases;
        public List<IStatement>? Default;
    }
}
