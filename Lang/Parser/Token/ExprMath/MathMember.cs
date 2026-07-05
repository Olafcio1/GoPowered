using GoPowered.Lang.Parser.Token.Expr;

namespace GoPowered.Lang.Parser.Token.ExprMath
{
    public record MathMember(
        MathMember.IOperation Type,
        Expression Value
    )
    {
        public interface IOperation : IAvoidSerialization;

        public readonly struct Arithmetic : IOperation
        {
            private Arithmetic(string display) { this.display = display; }
            private readonly string display;

            public override string ToString() { return display; }

            public static readonly Arithmetic Add = new("Add");
            public static readonly Arithmetic Subtract = new("Subtract");
            public static readonly Arithmetic Multiply = new("Multiply");
            public static readonly Arithmetic Divide = new("Divide");
            public static readonly Arithmetic Modulus = new("Modulus");
        }
        public readonly struct Bitwise : IOperation
        {
            private Bitwise(string display) { this.display = display; }
            private readonly string display;

            public override string ToString() { return display; }

            public static readonly Bitwise And = new("And");
            public static readonly Bitwise Or = new("Or");
            public static readonly Bitwise Xor = new("Xor");
            public static readonly Bitwise ShiftLeft = new("ShiftLeft");
            public static readonly Bitwise ShiftRight = new("ShiftRight");
        }
    }
}
