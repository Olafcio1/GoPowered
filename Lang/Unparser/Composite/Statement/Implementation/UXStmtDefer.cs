using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected virtual partial string HandleDefer(StmtDefer stmt)
        {
            var output = "defer ";

            output += HandleStatement(stmt.Expr);

            return output;
        }
    }
}
