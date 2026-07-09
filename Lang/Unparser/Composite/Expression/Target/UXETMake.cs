using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleMake(IExpressionTarget target, ref string output)
        {
            if (target is ETMake cast)
            {
                output += "make(";
                output += HandleType(cast.Type);

                foreach (var arg in cast.Values)
                {
                    output += ", ";
                    output += HandleAnyExpression(arg);
                }

                output += ")";

                return true;
            }

            return false;
        }
    }
}
