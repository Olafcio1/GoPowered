using GoPowered.Lang.Parser;

namespace GoPowered.Base
{
    public abstract class BaseParser<TokenType, ExceptionType>
    {
        protected readonly List<TokenType> input;
        protected int index;

        /**
         * What the helly I used the primary constructor and
         * then it fucked the other day
         *
         * I'm NOT using it again 🥀
         **/
        public BaseParser(List<TokenType> input)
        {
            this.input = input;
        }

        protected bool ReachedEOF()
        {
            return index >= input.Count;
        }

        protected TokenType Consume()
        {
            return input[index++];
        }

        protected T Consume<T>() where T : TokenType
        {
            var tok = Consume();
            var want = typeof(T).Name;

            #pragma warning disable CS8602
            var have = tok.GetType().Name;
            #pragma warning restore CS8602

            if (have != want)
                throw (Exception) typeof(ExceptionType).GetConstructor([
                    typeof(string)
                ])!.Invoke([
                    "expected '" + want + "', got '" + have + "'"
                ]);

            return (T)tok;
        }

        protected void Require(TokenType token, string humanValue)
        {
            if (!Now([(null, token)], true))
                throw new ParserError("expected " + humanValue);
        }

        protected TokenType Peek(int after)
        {
            return input[index + after];
        }

        protected abstract string TypeOf(TokenType token);

        protected bool Now((string?, TokenType?)[] types, bool consume = false)
        {
            var i = 0;
            foreach (var (type, token) in types)
            {
                var have = Peek(i++);
                var haveType = have.GetType();

                if (token == null)
                {
                    if (TypeOf(have) != type)
                        return false;
                }
                else
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
