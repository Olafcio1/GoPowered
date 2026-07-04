namespace GoPowered.Lang.Parser.Token.ExprLogic
{
    public record LEither(ICondition A, ICondition B)
                : ICondition;
}
