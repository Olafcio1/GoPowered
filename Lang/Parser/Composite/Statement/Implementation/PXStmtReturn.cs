using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtReturn ParseReturn()
        {
            if (Now([("newline", null)], false))
            {
                return new StmtReturn(null);
            }
            else
            {
                var values = new List<IAnyExpression>();

                do
                {
                    var expr = ParseExpression();
                    values.Add(expr);
                } while (Now([(null, Operator.Comma.ToToken())], true));

                return new StmtReturn(values);
            }
        }
    }
}
