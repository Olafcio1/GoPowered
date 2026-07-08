using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial IStatement ParseVar()
        {
            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var assignment = new StmtMultiAssign([]);

                while (true)
                {
                    ConsumeNewlines();

                    if (Now([(null, Operator.RParen.ToToken())], true))
                        break;

                    ParseVarDefinition(out var name, out var value, out var type);

                    assignment.Variables.Add(new Assignment(name, value, type));
                }

                return assignment;
            }
            else
            {
                ParseVarDefinition(out var name, out var value, out var type);

                return new StmtAssign(name, value, type);
            }
        }

        private void ParseVarDefinition(out string name, out IAnyExpression? value, out IType? type)
        {
            name = Consume<LTLiteral>().Value;

            value = null;
            type = null;

            if (!Now([(null, Operator.Set.ToToken())], false))
                type = ParseType();

            if (Now([(null, Operator.Set.ToToken())], true))
                value = ParseExpression();
        }
    }
}
