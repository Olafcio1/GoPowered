using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected virtual partial string HandleForRange(StmtForRange stmt)
        {
            var output = "for ";

            if (stmt.Variables != null)
            {
                var first = true;

                foreach (var variable in stmt.Variables)
                {
                    if (first)
                        first = false;
                    else output += ", ";

                    output += variable ?? "_";
                }

                output += " := ";
            }

            output += "range ";
            output += HandleAnyExpression(stmt.Iterator);
            output += HandleCode(stmt.Effect);

            return output;
        }
    }
}
