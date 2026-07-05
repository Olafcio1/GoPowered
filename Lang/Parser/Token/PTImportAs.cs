namespace GoPowered.Lang.Parser.Token
{
    public record PTImportAs(string Package, string? Alias) : IParserToken;
}
