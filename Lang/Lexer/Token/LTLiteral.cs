namespace GoPowered.Lang.Lexer.Token
{
    public record LTLiteral(string Value) : ILexerToken
    {
        public string Type() => "literal";
    }
}
