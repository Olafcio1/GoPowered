using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleSwitch(StmtSwitch stmt)
        {
            var output = "";

            output += "switch {";

            foreach (var @case in stmt.Cases)
            {
                output += "\n\tcase ";
                output += HandleAnyExpression(@case.Condition);
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
