using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleCast(IExpressionPart part, ref string output)
        {
            if (part is EPCast square)
            {
                output += ".(";
                output += HandleType(square.Type);
                output += ")";

                return true;
            }

            return false;
        }
    }
}
