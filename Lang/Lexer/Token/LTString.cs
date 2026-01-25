namespace GoPowered.Lang.Lexer.Token
{
    public record LTString(string Value) : ILexerToken
    {
        public string Type() => "string";
    }
}
