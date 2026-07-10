using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleReceive(IExpressionTarget target, ref string output)
        {
            if (target is ETReceive cast)
            {
                output += "<- ";
                output += HandleAnyExpression(cast.Expr);

                return true;
            }

            return false;
        }
    }
}
