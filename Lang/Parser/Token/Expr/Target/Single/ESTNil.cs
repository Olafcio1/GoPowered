namespace GoPowered.Lang.Parser.Token.Expr.Target.Single
{
    public record ESTNil : IExpressionTarget
    {
        public static readonly ESTNil INSTANCE = new ESTNil();
        private ESTNil() {}
    }
}
