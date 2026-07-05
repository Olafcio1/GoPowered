using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETMake? ParseMake(bool allowInit, bool constant)
        {
            if (Now([(null, Keyword.MAKE.ToToken())], true))
            {
                Require(Operator.LParen.ToToken(), "'('");

                var type = ParseType()!;
                var args = new List<IAnyExpression>();

                while (true)
                {
                    if (Now([(null, Operator.RParen.ToToken())], true))
                        break;

                    Require(Operator.Comma.ToToken(), "','");

                    args.Add(ParseExpression(allowInit: allowInit, constant: constant));
                }

                return new ETMake(type, args);
            }

            return null;
        }
    }
}
