using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleSelect(StmtSelect stmt)
        {
            var output = "";

            output += "select {";

            foreach (var @case in stmt.Cases)
            {
                output += "\n\tcase ";
                output += HandleStatement(@case.Messenger);
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
