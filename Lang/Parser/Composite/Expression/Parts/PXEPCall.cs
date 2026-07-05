using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseCall(List<IExpressionPart> parts)
        {
            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var args = new List<Parameter>();

                HandleList(Operator.RParen, () =>
                {
                    var value = ParseExpression();
                    var ellipsis = Now([(null, Operator.Ellipsis.ToToken())], true);

                    args.Add(new Parameter(value, ellipsis));
                });

                parts.Add(new EPCall(args));
                return true;
            }

            return false;
        }
    }
}
