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

                HandleList(Operator.RCurly, () =>
                {
                    var key = Consume<LTLiteral>().Value;
                    Require(Operator.Colon.ToToken(), "':'");
                    var value = ParseExpression();

                    token.Fields[key] = value;
                });

                return token;
            }

            return null;
        }
    }
}
