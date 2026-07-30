using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleSetExtract(StmtExtractSet stmt)
        {
            var output = "";
            var first = true;

            foreach (var expr in stmt.ExtractTo)
            {
                if (first)
                    first = false;
                else output += ", ";

                output += HandleAnyExpression(expr);
            }

            output += " := ";

            first = true;

            foreach (var expr in stmt.ExtractFrom)
            {
                if (first)
                    first = false;
                else output += ", ";

                output += HandleAnyExpression(expr);
            }

            return output;
        }
    }
}
