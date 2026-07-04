using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.ExprMath
{
    public record MathMember(
        MathMember.TypeEnum Type,
        Expression Value
    )
    {
        public enum TypeEnum
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulus
        }
    }
}
