using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETMap? ParseMap()
        {
            if (Now([(null, Keyword.MAP.ToToken())], true))
            {
                Require(Operator.LSquare.ToToken(), "'['");
                var keyType = ParseType()!;
                Require(Operator.RSquare.ToToken(), "']'");

                var valueType = ParseType()!;
                var token = new ETMap(keyType, valueType, []);

                Require(Operator.LCurly.ToToken(), "'{'");

                HandleList(Operator.RCurly, () =>
                {
                    var key = ParseExpression();
                    Require(Operator.Colon.ToToken(), "':'");
                    var value = ParseExpression();

                    token.Values[key] = value;
                });

                return token;
            }

            return null;
        }
    }
}
