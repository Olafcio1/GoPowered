using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleChannelSend(StmtChannelSend stmt)
        {
            var output = "";

            output += HandleAnyExpression(stmt.Channel);
            output += " <- ";
            output += HandleAnyExpression(stmt.Message);

            return output;
        }
    }
}
