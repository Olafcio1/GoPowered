using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtDefer ParseDefer()
        {
            var expr = ParseObjectExpression();
            if (expr.Parts == null || expr.Parts[^1] is not EPCall)
                throw new ParserError("Expected a function call after 'defer'");

            return new StmtDefer(expr);
        }
    }
}
