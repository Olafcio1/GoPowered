using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleSet(StmtSet stmt)
        {
            var output = "";

            output += HandleAnyExpression(stmt.Name);
            output += " = ";
            output += HandleAnyExpression(stmt.Value);

            return output;
        }
    }
}
