namespace GoPowered.Lang.Lexer.Token
{
    public record LTNewLine : ILexerToken
    {
        public static readonly LTNewLine INSTANCE = new LTNewLine();

        private LTNewLine() {}

        public string Type()
        {
            return "newline";
        }
    }
}
