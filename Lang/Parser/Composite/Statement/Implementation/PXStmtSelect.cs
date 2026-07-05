using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Select;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtSelect ParseSelect()
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            var token = new StmtSelect{
                                          Cases = [],
                                          Default = null
                                      };

            List<IStatement>? branch = null;
            var @default = false;

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                if (Now([(null, Keyword.CASE.ToToken())], true)) {
                    var stmt = ParseStatement();

                    #pragma warning disable CS0642

                    if (stmt is StmtAssign assign)
                    {
                        if (assign.Value is not Expression expr || expr.Singular || expr.Parts!.Count != 0 || expr.Target is not ETReceive)
                            throw new ParserError("A select case can only receive from a channel within an assignment");
                    }
                    else if (stmt is StmtExtractAssign assign2)
                    {
                        if (assign2.Value is not Expression expr || expr.Singular || expr.Parts!.Count != 0 || expr.Target is not ETReceive)
                            throw new ParserError("A select case can only receive from a channel within an assignment");
                    }
                    else if (stmt is StmtChannelSend);
                    else throw new ParserError("Expected a channel read or write");

                    #pragma warning restore CS0642

                    Require(Operator.Colon.ToToken(), "':'");

                    branch = [];
                    token.Cases.Add(new SelectCase(stmt, branch));

                    continue;
                }
                else if (Now([(null, Keyword.DEFAULT.ToToken())], true))
                {
                    if (@default)
                        throw new ParserError("A default case has already been specified");

                    Require(Operator.Colon.ToToken(), "':'");

                    branch = [];
                    token.Default = branch;

                    @default = true;

                    continue;
                }
                else if (branch == null)
                {
                    throw new ParserError("A select case must start with a case or default section");
                }

                branch.Add(ParseStatement());
            }

            return token;
        }
    }
}
