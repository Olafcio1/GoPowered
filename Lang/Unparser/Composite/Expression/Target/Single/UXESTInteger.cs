using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleESTInteger(ESTInteger integer)
        {
            return integer.Value.ToString();
        }
    }
}
