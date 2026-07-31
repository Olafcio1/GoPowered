using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleNew(IExpressionPart part, ref string output)
        {
            if (part is EPNew @new)
            {
                output += "{";

                bool first = true;

                foreach (var parameter in @new.Positional)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += HandleAnyExpression(parameter);
                }

                foreach (var (key, parameter) in @new.Keyword)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += key;
                    output += ": ";
                    output += HandleAnnotatedExpression(parameter);
                }

                output += "}";

                return true;
            }

            return false;
        }
    }
}
