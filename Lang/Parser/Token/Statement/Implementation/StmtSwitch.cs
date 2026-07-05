using GoPowered.Lang.Parser.Token.Statement.Implementation.Switch;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run code for the first case that the condition of matches.
     */
    public class StmtSwitch : IStatement
    {
        public required List<SwitchCase> Cases;
        public List<IStatement>? Default;
    }
}
