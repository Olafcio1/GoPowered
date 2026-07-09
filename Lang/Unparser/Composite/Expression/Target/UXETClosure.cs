using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleClosure(IExpressionTarget target, ref string output)
        {
            if (target is ETClosure cast)
            {
                output += "func";

                if (cast.Generics != null)
                    output += HandleFuncGenerics(cast.Generics);

                output += HandleFuncArguments(cast.Args);

                if (cast.Returns != null)
                    output += HandleFuncReturns(cast.Returns);

                output += HandleCode(cast.Body);

                return true;
            }

            return false;
        }
    }
}
