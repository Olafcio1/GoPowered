using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Parser.Type.Go
{
    public record ArrayType(IAnyExpression Amount, IType ElementType) : IType;
}
