using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleForLoop(StmtForLoop stmt)
        {
            var output = "for ";

            if (stmt.Initial != null)
            {
                output += HandleStatement(stmt.Initial);
                output += "; ";
            }

            output += HandleAnyExpression(stmt.Condition);

            if (stmt.After != null)
            {
                output += "; ";
                output += HandleStatement(stmt.After);
            }

            output += HandleCode(stmt.IterationEffect);

            return output;
        }
    }
}
