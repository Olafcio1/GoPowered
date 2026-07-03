namespace GoPowered.Lang.Parser.Token.Object
{
    public record PTTypeInterface(
        string Name,
        List<FunctionSignature> Methods,
        List<string> Inherits
    ) : IParserToken;
}
