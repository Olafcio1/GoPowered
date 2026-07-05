using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial IStatement ParseFor()
        {
            var i = 0;
            var collectVariables = false;

            while (true)
            {
                if (
                        Peek(i) is LTLiteral &&
                        Peek(i + 1) is LTOperator op && op.Value == Operator.Comma
                )
                {
                    i += 2;
                    continue;
                }
                else if (
                        Peek(i) is LTLiteral &&
                        Peek(i + 1) is LTOperator op2 &&
                        op2.Value == Operator.Assign
                )
                {
                    i += 2;
                    collectVariables = true;
                    break;
                }
                else
                {
                    break;
                }
            }

            List<string?>? variables = null;

            if (collectVariables)
            {
                variables = [];

                var comma = false;

                while (true)
                {
                    if (Now([(null, Operator.Assign.ToToken())], true))
                        break;
                    else if (!comma)
                        comma = true;
                    else Require(Operator.Comma.ToToken(), "','");

                    var name = Consume<LTLiteral>().Value;
                    if (name == "_")
                        name = null;

                    variables.Add(name);
                }
            }

            if (Now([(null, Keyword.RANGE.ToToken())], true))
            {
                var rangeThrough = ParseObjectExpression(false);
                var effect = ParseCode();

                return new StmtForRange(variables, rangeThrough, effect);
            }
            else
            {
                IAnyExpression? initial = null;

                if (variables != null)
                {
                    initial = ParseExpression();
                    Require(Operator.Semicolon.ToToken(), "';'");
                }

                var cond = ParseCondition(allowInit: false);

                IStatement? after = null;

                if (initial != null)
                    Require(Operator.Semicolon.ToToken(), "';'");

                if (!Now([(null, Operator.LCurly.ToToken())], false))
                {
                    after = ParseStatement();

                    if (after is StmtForLoop || after is StmtForRange || after is StmtIf || after is StmtReturn)
                        throw new ParserError("The for-loop post-iteration statement cannot be for, if or return");
                }

                var effect = ParseCode();

                return new StmtForLoop(variables == null
                                            ? null
                                            : new StmtExtractAssign(variables, initial, null), (Condition)cond, after, effect);
            }
        }
    }
}
