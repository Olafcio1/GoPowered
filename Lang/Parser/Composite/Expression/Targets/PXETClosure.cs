using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETClosure? ParseClosure()
        {
            if (Now([(null, Keyword.FUNCTION.ToToken())], true))
            {
                ParseFunctionSignature(out var args, out var returns, out var generics);

                var body = ParseCode();
                return new ETClosure(args, body, returns, generics);
            }

            return null;
        }
    }
}
