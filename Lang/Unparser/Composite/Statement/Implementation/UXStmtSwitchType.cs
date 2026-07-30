using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleSwitchType(StmtSwitchType stmt)
        {
            var output = "";

            output += "switch ";
            output += HandleAnyExpression(stmt.Value);
            output += " {";

            foreach (var @case in stmt.Cases)
            {
                output += "\n\tcase ";
                output += HandleType(@case.Expectation);
                output += ":";
                output += HandleCode(@case.Effect);
            }

            if (stmt.Default != null)
            {
                output += "\n\tdefault:";
                output += HandleCode(stmt.Default);
            }

            output += "}";

            return output;
        }
    }
}
