using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleExpression(Expression expr)
        {
            var output = "";

            if (expr.Pointers > 0)
            {
                for (int i = 0; i < expr.Pointers; i++)
                    output += "*";
            }
            else
            {
                for (int i = 0; i < expr.Pointers; i++)
                    output += "&";
            }

            throw new UnparserError("Unexpected expression target '" + TypeOf(expr.Target).Substring(3) + "'");
        }
    }
}
