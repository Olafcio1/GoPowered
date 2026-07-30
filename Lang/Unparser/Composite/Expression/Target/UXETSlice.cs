using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleSlice(IExpressionTarget target, ref string output)
        {
            if (target is ETSlice cast)
            {
                output += "[]";
                output += HandleType(cast.ElementType);

                output += "{";

                var first = true;

                foreach (var arg in cast.Values)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += HandleAnyExpression(arg);
                }

                output += "}";

                return true;
            }

            return false;
        }
    }
}
