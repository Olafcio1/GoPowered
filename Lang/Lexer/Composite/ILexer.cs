namespace GoPowered.Lang.Lexer.Composite
{
    public interface ILexer
    {
        char Peek(int index = 0);

        char Consume();

        void Skip(int count = 1);

        void AddToken(ILexerToken token);

        bool ReachedEOF(int further = 0);

        bool Now(char ch);

        bool Now(char ch, int count = 1);
    }
}
