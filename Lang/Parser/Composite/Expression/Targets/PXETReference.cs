using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETReference? ParseReference()
        {
            if (Now([("literal", null)], false))
            {
                var literal = Consume<LTLiteral>().Value;
                return new ETReference(literal);
            }

            return null;
        }
    }
}
