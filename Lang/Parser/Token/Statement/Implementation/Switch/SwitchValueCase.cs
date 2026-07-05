namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Switch
{
    public record SwitchValueCase(IAnyExpression Expectation, List<IStatement> Effect);
}
