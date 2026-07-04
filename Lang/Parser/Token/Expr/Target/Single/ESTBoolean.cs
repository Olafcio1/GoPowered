namespace GoPowered.Lang.Parser.Token.Expr.Target.Single
{
    public record ESTBoolean : IExpressionTarget, IAvoidSerialization
    {
        public static readonly ESTBoolean TRUE = new(true);
        public static readonly ESTBoolean FALSE = new(false);

        private readonly bool value;
        private ESTBoolean(bool value) {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
