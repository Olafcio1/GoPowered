namespace GoPowered.Lang.Lexer.Token
{
    public record LTInteger(long Value) : ILexerToken
    {
        public string Type() => "integer";
    }
}
