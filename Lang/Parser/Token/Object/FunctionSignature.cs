namespace GoPowered.Lang.Parser.Token.Object
{
    public record FunctionSignature(
        string Name,
        List<Argument> Args,
        List<ReturnValue>? Returns
    ) : IParserToken;
}
