using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected virtual partial string HandleExtractAssign(StmtExtractAssign stmt)
        {
            var output = "";
            var first = true;

            foreach (var name in stmt.Names)
            {
                if (first)
                    first = false;
                else output += ", ";

                output += name ?? "_";
            }

            if (stmt.Type != null)
            {
                output += " ";
                output += HandleType(stmt.Type);
            }

            if (stmt.Value != null)
            {
                output += " := ";
                output += HandleAnyExpression(stmt.Value);
            }

            return output;
        }
    }
}
