using GoPowered.Lang.Parser.Token.Statement.Implementation.Select;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to run code for the message that sent or was received the fastest across multiple options.
     */
    public class StmtSelect : IStatement
    {
        public required List<SelectCase> Cases;
        public List<IStatement>? Default;
    }
}
