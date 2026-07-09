using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;

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

            if (expr.Target is ESTBoolean boolean)
                output += HandleESTBoolean(boolean);
            else if (expr.Target is ESTInteger integer)
                output += HandleESTInteger(integer);
            else
                throw new UnparserError("Unexpected expression target '" + TypeOf(expr.Target).Substring(3) + "'");

            return output;
        }

        protected partial string HandleESTBoolean(ESTBoolean boolean);
        protected partial string HandleESTInteger(ESTInteger integer);
    }
}
