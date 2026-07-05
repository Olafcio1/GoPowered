using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Switch;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtSwitchValue ParseValueSwitch()
        {
            var isValue = ParseExpression(allowInit: false);

            Require(Operator.LCurly.ToToken(), "'{'");

            var token = new StmtSwitchValue{
                                               Value = isValue,
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
                    var expectation = ParseExpression();

                    Require(Operator.Colon.ToToken(), "':'");

                    branch = [];
                    token.Cases.Add(new SwitchValueCase(expectation, branch));

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
                    throw new ParserError("A switch case must start with a case or default section");
                }

                branch.Add(ParseStatement());
            }

            return token;
        }
    }
}
