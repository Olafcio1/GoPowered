using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Parser
{
    public class Parser
    {
        protected readonly List<ILexerToken> input;
        protected int index;

        public readonly List<IParserToken> output;
        public string package;

        public Parser(List<ILexerToken> input)
        {
            this.input = input;
            this.index = 0;
            this.output = [];
        }

        public void Parse()
        {
            Require(Keyword.PACKAGE.ToToken(), "'package'");
            package = Consume<LTLiteral>().Value;

            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.IMPORT.ToToken())], true))
                {
                    output.Add(new PTImport(Consume<LTString>().Value));
                }
                else break;
            }
        }

        protected bool ReachedEOF() {
            return index >= input.Count;
        }

        protected ILexerToken Consume()
        {
            return input[index++];
        }

        protected T Consume<T>() where T : ILexerToken
        {
            var tok = Consume();

            var want = typeof(T).Name;
            var have = tok.GetType().Name;

            if (have != want)
                throw new ParserError("expected '" + want + "', got '" + have + "'");

            return (T) tok;
        }

        protected void Require(ILexerToken token, string humanValue)
        {
            if (!Now([(null, token)], true))
                throw new ParserError("expected " + humanValue);
        }

        protected ILexerToken Peek(int after)
        {
            return input[index + after];
        }

        protected bool Now((string?, ILexerToken?)[] types, bool consume = false)
        {
            var i = 0;
            foreach (var (type, token) in types)
            {
                var have = Peek(i++);
                var haveType = have.GetType();

                if (token == null)
                {
                    if (haveType.Name != type)
                        return false;
                } else
                {
                    var tokenType = token.GetType();
                    if (haveType.Name != tokenType.Name)
                        return false;

                    if (!token.GetType().GetProperties().All(el =>
                            {
                                var haveProp = haveType.GetProperty(el.Name);
                                if (haveProp == null)
                                    return false;

                                var wantValue = el.GetValue(token)!;
                                var haveValue = haveProp.GetValue(have);

                                return wantValue.Equals(haveValue);
                            })
                    ) return false;
                }
            }

            if (consume)
                index += i;

            return true;
        }
    }
}
