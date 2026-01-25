namespace GoPowered.Lang.Lexer.Token
{
    public record LTFloat(double Value) : ILexerToken
    {
        public string Type() => "float";
    }
}
