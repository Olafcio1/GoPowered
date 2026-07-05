using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETImplicitStruct? ParseImplicitStruct()
        {
            if (Now([(null, Operator.LCurly.ToToken())], true))
            {
                var token = new ETImplicitStruct([]);

                var comma = false;
                var newlines = false;

                while (true)
                {
                    ConsumeNewlines(ref newlines);

                    if (!newlines && Now([(null, Operator.RCurly.ToToken())], true))
                        break;
                    else if (comma)
                        Require(Operator.Comma.ToToken(), "','");
                    else comma = true;

                    ConsumeNewlines(ref newlines);

                    if (Now([(null, Operator.RCurly.ToToken())], true))
                        break;

                    ConsumeNewlines(ref newlines);

                    var key = Consume<LTLiteral>().Value;
                    Require(Operator.Colon.ToToken(), "':'");
                    var value = ParseExpression();

                    token.Fields[key] = value;
                }

                return token;
            }

            return null;
        }
    }
}
