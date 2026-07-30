using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected virtual partial string HandleBreak(StmtBreak stmt)
        {
            return "break";
        }
    }
}
