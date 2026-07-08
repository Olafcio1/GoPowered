using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

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

                    if (stmt is StmtBreak @break)
                    {
                        output += HandleBreak(@break);
                    }
                    else
                    {
                        throw new UnparserError("Unexpected statement '" + TypeOf(stmt) + "'");
                    }
                }

                output += "\n";
            }

            output += "}";

            return output;
        }

        protected partial string HandleBreak(StmtBreak stmt);
    }
}
