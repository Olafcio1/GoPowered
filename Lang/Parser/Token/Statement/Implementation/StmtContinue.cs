namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to continue a for-loop.
     */
    public class StmtContinue : IStatement, IAvoidSerialization
    {
        public static readonly StmtContinue INSTANCE = new();

        internal StmtContinue() {}
    }
}
