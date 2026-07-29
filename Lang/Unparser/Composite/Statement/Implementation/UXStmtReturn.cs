using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleReturn(StmtReturn stmt)
        {
            var output = "return";

            if (stmt.Values != null)
            {
                foreach (var value in stmt.Values)
                {
                    output += " ";
                    output += HandleAnyExpression(value);
                }
            }

            return output;
        }
    }
}
