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

                 if (HandleSingular(expr.Target, ref output));
            else if (HandleClosure(expr.Target, ref output));
            else
                throw new UnparserError("Unexpected expression target '" + TypeOf(expr.Target).Substring(3) + "'");

            return output;
        }

        protected partial bool HandleClosure(IExpressionTarget target, ref string output);

        protected bool HandleSingular(IExpressionTarget target, ref string output)
        {
            if (target is ESTBoolean boolean)
                output += HandleESTBoolean(boolean);
            else if (target is ESTInteger integer)
                output += HandleESTInteger(integer);
            else if (target is ESTFloat @float)
                output += HandleESTFloat(@float);
            else if (target is ESTChar @char)
                output += HandleESTChar(@char);
            else if (target is ESTString @string)
                output += HandleESTString(@string);
            else if (target == ESTNil.INSTANCE)
                output += HandleESTNil();
            else return false;

            return true;
        }

        protected partial string HandleESTBoolean(ESTBoolean boolean);
        protected partial string HandleESTInteger(ESTInteger integer);
        protected partial string HandleESTFloat(ESTFloat @float);
        protected partial string HandleESTChar(ESTChar @char);
        protected partial string HandleESTString(ESTString @string);
        protected partial string HandleESTNil();
    }
}
