using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleMap(IExpressionTarget target, ref string output)
        {
            if (target is ETMap cast)
            {
                output += "[";
                output += HandleType(cast.KeyType);
                output += "]";
                output += HandleType(cast.ValueType);

                output += "{";

                var first = true;

                foreach (var (key, value) in cast.Values)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += HandleAnyExpression(key);
                    output += ": ";
                    output += HandleAnyExpression(value);
                }

                output += ")";

                return true;
            }

            return false;
        }
    }
}
