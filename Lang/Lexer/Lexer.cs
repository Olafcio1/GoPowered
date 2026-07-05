using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public partial class Lexer
    {
        protected readonly string input;
        protected int index;
        public readonly List<ILexerToken> output;

        public Lexer(string input) {
            this.input = input;
            this.index = 0;
            this.output = [];
        }

        public List<ILexerToken> Lex()
        {
            while (!ReachedEOF())
            {
                #pragma warning disable CS0642
                if (LexNumber());
                else if (Now("//")) {
                    // Comments
                    while (Consume() != '\n');

                    output.Add(LTNewLine.INSTANCE);
                }
                else if (Now("/*")) {
                    // Multiline comments
                    while (!Now("*/"))
                        Consume();

                    output.Add(LTNewLine.INSTANCE);
                }
                else if (LexOperator());
                else if (LexLiteral());
                else if (LexString());
                else if (LexChar());
                else if (Now('\r') || Now('\n'))
                    // Newlines
                    output.Add(LTNewLine.INSTANCE);
                else if (Peek() <= 32)
                    // Control keys
                    index++;
                else throw new LexerError("unexpected '" + Consume() + "'");
                #pragma warning restore CS0642
            }

            return output;
        }

        protected char Consume()
        {
            return input[index++];
        }

        protected string Consume(int length)
        {
            return input.Substring(index, index += length);
        }

        protected char Peek(int after = 0)
        {
            return input[index + after];
        }

        protected string PeekString(int length, int after = 0)
        {
            var ind = index + after;

            length = Math.Min(length, input.Length - (ind + length));
            length = Math.Max(0, length);

            return input.Substring(ind, length);
        }

        protected bool Now(char ch, int after = 0)
        {
            if (Peek(after) == ch) {
                index += 1 + after;
                return true;
            } else {
                return false;
            }
        }

        protected bool Now(string value, int after = 0)
        {
            var len = value.Length;

            if (PeekString(len, after) == value)
            {
                index += len + after;
                return true;
            } else
            {
                return false;
            }
        }

        protected bool ReachedEOF(int further = 0) {
            return index + further >= input.Length;
        }

        protected bool IsFirst()
        {
            return index == 0;
        }

        protected void Skip(int count = 1)
        {
            index += count;
        }

        protected void AddToken(ILexerToken token)
        {
            output.Add(token);
        }
    }
}
