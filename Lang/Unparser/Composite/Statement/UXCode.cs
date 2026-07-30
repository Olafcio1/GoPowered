using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleCode(List<IStatement> code)
        {
            var output = " {";

            if (code.Count != 0)
            {
                foreach (var stmt in code)
                {
                    output += "\n\t";
                    output += HandleStatement(stmt);
                }

                output += "\n";
            }

            output += "}";

            return output;
        }

        private string HandleStatement(IStatement stmt)
        {
            if (stmt is StmtAssign assign)
            {
                return HandleAssign(assign);
            }
            else if (stmt is StmtExtractAssign assignex)
            {
                return HandleExtractAssign(assignex);
            }
            else if (stmt is StmtConst @const)
            {
                return HandleConst(@const);
            }
            else if (stmt is StmtBreak @break)
            {
                return HandleBreak(@break);
            }
            else if (stmt is StmtContinue @continue)
            {
                return HandleContinue(@continue);
            }
            else if (stmt is StmtExpression expr)
            {
                return HandleAnyExpression(expr.Expr);
            }
            else if (stmt is StmtReturn @return)
            {
                return HandleReturn(@return);
            }
            else if (stmt is StmtSet set)
            {
                return HandleSet(set);
            }
            else if (stmt is StmtDefer defer)
            {
                return HandleDefer(defer);
            }
            else if (stmt is StmtIf @if)
            {
                return HandleIf(@if);
            }
            else if (stmt is StmtForLoop forLoop)
            {
                return HandleForLoop(forLoop);
            }
            else if (stmt is StmtForRange forRange)
            {
                return HandleForRange(forRange);
            }
            else if (stmt is StmtClose close)
            {
                return HandleClose(close);
            }
            else
            {
                throw new UnparserError("Unexpected statement '" + TypeOf(stmt) + "'");
            }
        }

        protected partial string HandleAssign(StmtAssign stmt);
        protected partial string HandleExtractAssign(StmtExtractAssign stmt);
        protected partial string HandleConst(StmtConst stmt);
        protected partial string HandleBreak(StmtBreak stmt);
        protected partial string HandleContinue(StmtContinue stmt);
        protected partial string HandleReturn(StmtReturn stmt);
        protected partial string HandleSet(StmtSet stmt);
        protected partial string HandleDefer(StmtDefer stmt);
        protected partial string HandleIf(StmtIf stmt);
        protected partial string HandleForLoop(StmtForLoop stmt);
        protected partial string HandleForRange(StmtForRange stmt);
        protected partial string HandleClose(StmtClose stmt);
    }
}
