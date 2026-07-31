using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleImplicitStruct(IExpressionTarget target, ref string output)
        {
            if (target is ETImplicitStruct cast)
            {
                output += "{";

                bool first = true;

                foreach (var parameter in cast.Positional)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += HandleAnyExpression(parameter);
                }

                foreach (var (key, parameter) in cast.Keyword)
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
