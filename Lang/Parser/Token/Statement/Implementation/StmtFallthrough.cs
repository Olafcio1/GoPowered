namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to continue a switch.
     */
    public class StmtFallthrough : IStatement, IAvoidSerialization
    {
        public static readonly StmtFallthrough INSTANCE = new();

        internal StmtFallthrough() {}
    }
}
