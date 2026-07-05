namespace GoPowered.Lang.Parser.Token.Statement.Implementation.Select
{
    public record SelectCase(IStatement Messenger, List<IStatement> Effect);
}
