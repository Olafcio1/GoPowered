using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleNest(IExpressionTarget target, ref string output)
        {
            if (target is ETNest cast)
            {
                output += "(";
                output += HandleAnyExpression(cast.Expr);
                output += ")";

                return true;
            }

            return false;
        }
    }
}
