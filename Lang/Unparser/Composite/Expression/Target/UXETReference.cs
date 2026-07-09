using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleReference(IExpressionTarget target, ref string output)
        {
            if (target is ETReference cast)
            {
                output += cast.Name;

                return true;
            }

            return false;
        }
    }
}
