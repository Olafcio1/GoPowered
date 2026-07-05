using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.If;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtIf ParseIf()
        {
            var branches = new List<Branch>();
            var first = true;

            while (true)
            {
                if (first)
                    first = false;
                else
                {
                    var i = 0;
                    var ok = false;

                    while (!ReachedEOF(i))
                    {
                        if (Peek(i++) is LTNewLine)
                            continue;

                        if (!(Peek(i - 1) is LTKeyword kw1 && kw1.Value == Keyword.ELSE))
                            goto postBranches;

                        if (!(Peek(i) is LTKeyword kw2 && kw2.Value == Keyword.IF))
                            goto postBranches;

                        i++;
                        ok = true;

                        break;
                    }

                    if (!ok)
                        goto postBranches;

                    this.index += i;
                }

                var branch = new Branch();

                if (Assigning())
                {
                    branch.PreCond = ParseStatement();
                    Require(Operator.Semicolon.ToToken(), "';'");
                }

                var expr = ParseExpression(allowInit: false);
                var cond = expr is Condition cast
                            ? cast
                            : new Condition(expr, ConditionType.EQUAL, new Expression(ESTBoolean.TRUE, null, 0, Singular: true));

                var effect = ParseCode();

                branch.Cond = cond;
                branch.Effect = effect;

                branches.Add(branch);
            }

            postBranches:

            var otherwise = Now([(null, Keyword.ELSE.ToToken())], true)
                              ? ParseCode()
                              : null;

            return new StmtIf(branches, otherwise);
        }
    }
}
