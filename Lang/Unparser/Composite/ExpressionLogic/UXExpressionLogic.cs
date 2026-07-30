using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.ExprLogic;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleLogicExpression(ICondition expr)
        {
            if (expr is Condition cond)
            {
                var output = "";

                output += HandleAnyExpression(cond.Left);
                output += " ";

                output += HandleLogicOperation(cond.Type);

                output += " ";
                output += HandleAnyExpression(cond.Right);

                return output;
            }
            else if (expr is LBoth both)
            {
                var output = "";
                
                output += HandleAnyExpression(both.A);
                output += " && ";
                output += HandleAnyExpression(both.B);

                return output;
            }
            else if (expr is LEither either)
            {
                var output = "";

                output += HandleAnyExpression(either.A);
                output += " || ";
                output += HandleAnyExpression(either.B);

                return output;
            }

            throw new UnparserError("Expected a condition");
        }

        private string HandleLogicOperation(ConditionType op)
        {
                 if (op.Equals(ConditionType.EQUAL))            return "==";
            else if (op.Equals(ConditionType.NOT_EQUAL))        return "!=";
            else if (op.Equals(ConditionType.LESS_THAN))        return "<";
            else if (op.Equals(ConditionType.LESS_OR_EQUAL))    return "<=";
            else if (op.Equals(ConditionType.GREATER_THAN))     return ">";
            else if (op.Equals(ConditionType.GREATER_OR_EQUAL)) return ">=";

            throw new UnparserError("Expected a logic operation");
        }
    }
}
