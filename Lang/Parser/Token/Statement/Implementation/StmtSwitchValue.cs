using GoPowered.Lang.Parser.Token.Statement.Implementation.Switch;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run code for the first case that the value of matches the target expression.
     */
    public class StmtSwitchValue : IStatement
    {
        public required IAnyExpression Value;
        public required List<SwitchValueCase> Cases;
        public List<IStatement>? Default;
    }
}
