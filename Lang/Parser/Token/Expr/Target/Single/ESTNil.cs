namespace GoPowered.Lang.Parser.Token.Expr.Target.Single
{
    public record ESTNil : IExpressionTarget, IAvoidSerialization
    {
        public static readonly ESTNil INSTANCE = new ESTNil();
        private ESTNil() {}
    }
}
