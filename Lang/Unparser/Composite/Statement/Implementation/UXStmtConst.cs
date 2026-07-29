using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleConst(StmtConst stmt)
        {
            var output = "const ";

            if (stmt.Type != null)
            {
                output += HandleType(stmt.Type);
                output += " ";
            }

            output += stmt.Name ?? "_";

            if (stmt.Value != null)
            {
                output += " = ";
                output += HandleAnyExpression(stmt.Value);
            }

            return output;
        }
    }
}
