using GoPowered.Lang.Parser.Token.ExprLogic;

namespace GoPowered.Lang.Parser.Token.Statement.Implementation.If
{
    public record Branch
    {
        public IStatement PreCond;
        public Condition Cond;
        public List<IStatement> Effect;
    }
}
