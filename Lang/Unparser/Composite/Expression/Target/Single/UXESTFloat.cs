using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleESTFloat(ESTFloat @float)
        {
            return @float.Value.ToString();
        }
    }
}
