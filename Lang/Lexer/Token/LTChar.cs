namespace GoPowered.Lang.Lexer.Token
{
    public record LTChar(char Value) : ILexerToken
    {
        public string Type() => "char";
    }
}
