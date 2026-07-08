using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleESTBoolean(ESTBoolean boolean)
        {
            return boolean == ESTBoolean.TRUE
                    ? "true"
                    : "false";
        }
    }
}
