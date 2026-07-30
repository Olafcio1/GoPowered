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

        protected virtual string HandleStatement(IStatement stmt)
        {
            if (stmt is StmtAssign assign)
            {
                return HandleAssign(assign);
            }
            else if (stmt is StmtExtractAssign assignex)
            {
                return HandleExtractAssign(assignex);
            }
            else if (stmt is StmtExtractSet assignset)
            {
                return HandleSetExtract(assignset);
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
            else if (stmt is StmtFallthrough @fallthrough)
            {
                return HandleFallthrough(fallthrough);
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
            else if (stmt is StmtChannelSend channelSend)
            {
                return HandleChannelSend(channelSend);
            }
            else if (stmt is StmtSelect select)
            {
                return HandleSelect(select);
            }
            else if (stmt is StmtSwitch @switch)
            {
                return HandleSwitch(@switch);
            }
            else if (stmt is StmtSwitchValue switchValue)
            {
                return HandleSwitchValue(switchValue);
            }
            else if (stmt is StmtSwitchType switchType)
            {
                return HandleSwitchType(switchType);
            }
            else
            {
                throw new UnparserError("Unexpected statement '" + TypeOf(stmt) + "'");
            }
        }

        protected virtual partial string HandleAssign(StmtAssign stmt);
        protected virtual partial string HandleExtractAssign(StmtExtractAssign stmt);
        protected virtual partial string HandleSetExtract(StmtExtractSet stmt);
        protected virtual partial string HandleConst(StmtConst stmt);
        protected virtual partial string HandleBreak(StmtBreak stmt);
        protected virtual partial string HandleContinue(StmtContinue stmt);
        protected virtual partial string HandleFallthrough(StmtFallthrough stmt);
        protected virtual partial string HandleReturn(StmtReturn stmt);
        protected virtual partial string HandleSet(StmtSet stmt);
        protected virtual partial string HandleDefer(StmtDefer stmt);
        protected virtual partial string HandleIf(StmtIf stmt);
        protected virtual partial string HandleForLoop(StmtForLoop stmt);
        protected virtual partial string HandleForRange(StmtForRange stmt);
        protected virtual partial string HandleClose(StmtClose stmt);
        protected virtual partial string HandleChannelSend(StmtChannelSend stmt);
        protected virtual partial string HandleSelect(StmtSelect stmt);
        protected virtual partial string HandleSwitch(StmtSwitch stmt);
        protected virtual partial string HandleSwitchType(StmtSwitchType stmt);
        protected virtual partial string HandleSwitchValue(StmtSwitchValue stmt);
    }
}
