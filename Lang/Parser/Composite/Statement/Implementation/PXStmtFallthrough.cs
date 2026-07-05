using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtFallthrough ParseFallthrough()
        {
            return StmtFallthrough.INSTANCE;
        }
    }
}
