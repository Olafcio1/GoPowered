using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial StmtBreak ParseBreak()
        {
            return StmtBreak.INSTANCE;
        }
    }
}
