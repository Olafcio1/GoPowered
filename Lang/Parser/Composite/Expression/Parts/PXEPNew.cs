using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseNew(List<IExpressionPart> parts)
        {
            if (Now([(null, Operator.LCurly.ToToken())], true))
            {
                var positional = new List<IAnyExpression>();
                var keyword = new Dictionary<string, IAnyExpression>();

                HandleList(Operator.RCurly, () =>
                {
                    if (Now([("literal", null), (null, Operator.Colon.ToToken())], false))
                    {
                        var name = Consume<LTLiteral>().Value;
                        Require(Operator.Colon.ToToken(), "':'");
                        var value = ParseExpression();

                        keyword[name] = value;
                    }
                    else
                    {
                        if (keyword.Count > 0)
                            throw new ParserError("Cannot provide positional parameters after keyword parameters");

                        positional.Add(ParseExpression());
                    }
                });

                parts.Add(new EPNew(positional, keyword));

                return true;
            }

            return false;
        }
    }
}
