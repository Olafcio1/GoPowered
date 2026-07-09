using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleAssign(StmtAssign stmt)
        {
            var output = "var ";

            output += stmt.Name ?? "_";

            if (stmt.Type != null)
            {
                output += " ";
                output += HandleType(stmt.Type);
            }

            if (stmt.Value != null)
            {
                output += " = ";
                output += HandleAnyExpression(stmt.Value);
            }

            return output;
        }
    }
}
