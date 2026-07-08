using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Statement;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleCode(List<IStatement> code)
        {
            var output = " {";

            if (code.Count != 0)
            {
                foreach (var stmt in code)
                {
                    output += "\n\t";

                    throw new UnparserError("Unexpected statement '" + TypeOf(stmt) + "'");
                }

                output += "\n";
            }

            output += "}";

            return output;
        }
    }
}
