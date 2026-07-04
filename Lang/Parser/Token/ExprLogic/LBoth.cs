namespace GoPowered.Lang.Parser.Token.ExprLogic
{
    public record LBoth(ICondition A, ICondition B)
                : ICondition;
}
