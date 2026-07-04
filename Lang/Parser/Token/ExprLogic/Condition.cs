namespace GoPowered.Lang.Parser.Token.ExprLogic
{
    public record Condition(IAnyExpression Left, ConditionType Type, IAnyExpression Right)
                : IAnyExpression;
}
