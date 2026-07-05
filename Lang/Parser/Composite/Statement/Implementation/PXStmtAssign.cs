using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial IStatement ParseAssignment()
        {
            var names = new List<string>();

            do
            {
                names.Add(Consume<LTLiteral>().Value);
            } while (Now([(null, Operator.Comma.ToToken())], true));

            Require(Operator.Assign.ToToken(), "':='");

            var value = ParseExpression();

            return names.Count == 1
                    ? new StmtAssign(names[0], value, null)
                    : new StmtExtractAssign(names, value, null);
        }
    }
}
