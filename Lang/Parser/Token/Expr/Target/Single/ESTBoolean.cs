namespace GoPowered.Lang.Parser.Token.Expr.Target.Single
{
    public record ESTBoolean : IExpressionTarget
    {
        public static readonly ESTBoolean TRUE = new ESTBoolean();
        public static readonly ESTBoolean FALSE = new ESTBoolean();

        private ESTBoolean() {}
    }
}
