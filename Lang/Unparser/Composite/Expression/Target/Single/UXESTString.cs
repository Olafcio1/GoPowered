using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleESTString(ESTString @string)
        {
            return '"' + @string.Value
                                .Replace("'", "\\'")
                                .Replace("\n", "\\n")
                                .Replace("\r", "\\r")
                                .Replace("\t", "\\t")
                                .Replace("\b", "\\b")
                                .Replace("\a", "\\a")
                                .Replace("\f", "\\f")
                                .Replace("\e", "\\e") + '"';
        }
    }
}
