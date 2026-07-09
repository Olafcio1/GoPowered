using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.ExprMath;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleMathExpression(MathExpression expr)
        {
            var output = "";

            output += HandleAnyExpression(expr.Target);

            foreach (var member in expr.Members)
            {
                output += " ";
                output += HandleMathOperation(member.Type);
                output += " ";
                output += HandleAnyExpression(member.Value);
            }

            return output;
        }

        private string HandleMathOperation(MathMember.IOperation op)
        {
                 if (op.Equals(MathMember.Arithmetic.Add)) return "+";
            else if (op.Equals(MathMember.Arithmetic.Subtract)) return "-";
            else if (op.Equals(MathMember.Arithmetic.Multiply)) return "*";
            else if (op.Equals(MathMember.Arithmetic.Divide)) return "/";
            else if (op.Equals(MathMember.Arithmetic.Modulus)) return "%";

                 if (op.Equals(MathMember.Bitwise.And)) return "&";
            else if (op.Equals(MathMember.Bitwise.Or)) return "|";
            else if (op.Equals(MathMember.Bitwise.ShiftLeft)) return "<<";
            else if (op.Equals(MathMember.Bitwise.ShiftRight)) return ">>";
            else if (op.Equals(MathMember.Bitwise.Xor)) return "^";

            throw new UnparserError("Expected a math operation");
        }
    }
}
