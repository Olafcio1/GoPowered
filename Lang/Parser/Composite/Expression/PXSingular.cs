using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial IExpressionTarget? ParseSingularExpression(bool optional = false)
        {
            if (Now([("string", null)], false))
                return new ESTString(Consume<LTString>().Value);
            else if (Now([("char", null)], false))
                return new ESTChar(Consume<LTChar>().Value);
            else if (Now([("integer", null)], false))
                return new ESTInteger(Consume<LTInteger>().Value);
            else if (Now([("float", null)], false))
                return new ESTFloat(Consume<LTFloat>().Value);
            else if (Now([(null, Keyword.TRUE.ToToken())], true))
                return ESTBoolean.TRUE;
            else if (Now([(null, Keyword.FALSE.ToToken())], true))
                return ESTBoolean.FALSE;
            else if (Now([(null, Keyword.NIL.ToToken())], true))
                return ESTNil.INSTANCE;
            else if (optional)
                return null;
            else throw new ParserError("Expected a singular expression");
        }
    }
}
