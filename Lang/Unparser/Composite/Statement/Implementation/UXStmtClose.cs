using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleClose(StmtClose stmt)
        {
            var output = "close(";

            output += HandleAnyExpression(stmt.Expr);
            output += ")";

            return output;
        }
    }
}
