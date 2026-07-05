namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to break a for-loop.
     */
    public class StmtBreak : IStatement, IAvoidSerialization
    {
        public static readonly StmtBreak INSTANCE = new();

        internal StmtBreak() {}
    }
}
