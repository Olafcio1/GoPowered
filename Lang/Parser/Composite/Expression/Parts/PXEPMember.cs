using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseMember(List<IExpressionPart> parts)
        {
            if (Now([(null, Operator.Dot.ToToken())], true))
            {
                parts.Add(new EPMember(Consume<LTLiteral>().Value));
                return true;
            }

            return false;
        }
    }
}
