using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleContinue(StmtContinue stmt)
        {
            return "continue";
        }
    }
}
