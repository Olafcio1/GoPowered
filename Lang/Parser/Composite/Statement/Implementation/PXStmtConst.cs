using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtConst ParseConst()
        {
            IType? type = null;

            if (!Now([("literal", null)]))
                type = ParseType();

            var name = Consume<LTLiteral>().Value;
            Now([("newline", null)], consume: true);

            Require(Operator.Set.ToToken(), "'='");

            var value = ParseExpression(constant: true);

            return new StmtConst(name, type, value!);
        }
    }
}
