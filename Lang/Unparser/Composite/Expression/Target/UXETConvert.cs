using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleConvert(IExpressionTarget target, ref string output)
        {
            if (target is ETConvert cast)
            {
                output += cast.Name;
                output += "(";
                output += HandleAnyExpression(cast.Expr);
                output += ")";

                return true;
            }

            return false;
        }
    }
}
