using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtClose ParseClose()
        {
            Require(Operator.LParen.ToToken(), "'('");
            var expr = ParseObjectExpression();
            Require(Operator.RParen.ToToken(), "')'");

            return new StmtClose(expr);
        }
    }
}
