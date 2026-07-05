using GoPowered.Lang.Parser.Token.ExprLogic;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Switch
{
    public record SwitchCase(Condition Condition, List<IStatement> Effect);
}
