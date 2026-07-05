using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETSlice? ParseSlice()
        {
            if (Now([
                      (null, Operator.LSquare.ToToken()),
                      (null, Operator.RSquare.ToToken())
                    ], true)
            )
            {
                var elementType = ParseType()!;
                var token = new ETSlice(elementType, []);

                Require(Operator.LCurly.ToToken(), "'{'");

                HandleList(Operator.RCurly, () =>
                {
                    var value = ParseExpression();

                    token.Values.Add(value);
                });

                return token;
            }

            return null;
        }
    }
}
