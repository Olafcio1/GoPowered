using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser.Token.Expr.Target
{
    public record ETClosure(
        List<Argument> Args,
        List<IStatement> Body,
        List<ReturnValue>? Returns,
        Dictionary<string, IType>? Generics
    ) : IExpressionTarget;
}
