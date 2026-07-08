using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtConst ParseConst()
        {
            var name = Consume<LTLiteral>().Value;
            Now([("newline", null)], consume: true);
            Require(Operator.Set.ToToken(), "'='");
            var value = ParseExpression(constant: true);

            return new StmtConst(name, value!);
        }
    }
}
