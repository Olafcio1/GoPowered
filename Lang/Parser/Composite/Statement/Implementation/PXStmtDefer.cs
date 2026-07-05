using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtDefer ParseDefer()
        {
            var stmt = ParseStatement();

            if (stmt is StmtExpression cast)
            {
                var anyexpr = cast.Expr;
                if (anyexpr is not Expression expr || expr.Parts == null || expr.Parts[^1] is not EPCall)
                    throw new ParserError("Expected a function call/channel close after 'defer'");
            }
            else if (stmt is not StmtClose)
            {
                throw new ParserError("Expected a function call/channel close after 'defer'");
            }


            return new StmtDefer(stmt);
        }
    }
}
