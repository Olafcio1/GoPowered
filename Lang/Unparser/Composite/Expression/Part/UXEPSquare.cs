using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleSquare(IExpressionPart part, ref string output)
        {
            if (part is EPSquare square)
            {
                output += "[";

                if (square.Type != null)
                    output += HandleType(square.Type);
                else
                    output += HandleAnyExpression(square.Access!);

                output += "]";

                return true;
            }

            return false;
        }
    }
}
