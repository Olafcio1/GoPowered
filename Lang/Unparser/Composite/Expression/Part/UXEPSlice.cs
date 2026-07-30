using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleSlice(IExpressionPart part, ref string output)
        {
            if (part is EPSlice slice)
            {
                output += "[";

                if (slice.From != null)
                    output += HandleAnyExpression(slice.From);

                output += ":";

                if (slice.To != null)
                    output += HandleAnyExpression(slice.To);

                output += "]";

                return true;
            }

            return false;
        }
    }
}
