using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleIf(StmtIf stmt)
        {
            var output = "if ";
            var first = true;

            foreach (var branch in stmt.Branches)
            {
                if (first)
                    first = false;
                else output += " else if ";

                if (branch.PreCond != null)
                {
                    output += HandleStatement(branch.PreCond);
                    output += "; ";
                }

                output += HandleAnyExpression(branch.Cond);
                output += HandleCode(branch.Effect);
            }

            if (stmt.Else != null)
            {
                output += " else ";
                output += HandleCode(stmt.Else);
            }

            return output;
        }
    }
}
