using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseNew(List<IExpressionPart> parts, ref List<IType>? generics)
        {
            if (Now([(null, Operator.LCurly.ToToken())], true))
            {
                var positional = new List<IAnyExpression>();
                var keyword = new Dictionary<string, IAnyExpression>();

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
                }

                parts.Add(new EPNew(positional, keyword, generics));
                generics = null;

                return true;
            }

            return false;
        }
    }
}
