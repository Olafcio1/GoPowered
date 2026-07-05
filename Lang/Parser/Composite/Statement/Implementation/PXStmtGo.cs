using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtGo ParseGo()
        {
            var expr = ParseObjectExpression();
            if (expr.Parts == null || expr.Parts[^1] is not EPCall)
                throw new ParserError("Expected a function call after 'go'");

            return new StmtGo(expr);
        }
    }
}
