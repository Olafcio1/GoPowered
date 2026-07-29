using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Switch
{
    public record SwitchTypeCase(IType Expectation, List<IStatement> Effect);
}
