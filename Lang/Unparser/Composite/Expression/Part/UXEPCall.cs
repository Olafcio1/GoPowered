using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleCall(IExpressionPart part, ref string output)
        {
            if (part is EPCall call)
            {
                output += "(";

                bool first = true;

                foreach (var parameter in call.Parameters)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += HandleAnyExpression(parameter.Value);

                    if (parameter.Rest)
                        output += "...";
                }

                output += ")";

                return true;
            }

            return false;
        }
    }
}
