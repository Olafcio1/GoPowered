using System.ComponentModel;
using System.Reflection;

namespace GoPowered.Lang.Lexer.Token
{
    public record LTOperator(Operator Value) : ILexerToken;

    public enum Operator
    {
        // Parentheses
        [Description("(")] LParen,  [Description(")")] RParen,
        [Description("[")] LSquare, [Description("]")] RSquare,
        [Description("{")] LCurly,  [Description("}")] RCurly,
        // Arithmetic
        [Description("+")] Plus, //(Add)
        [Description("-")] Minus, //(Subtract)
        [Description("*")] Star, //(Multiply)
        [Description("/")] Slash, //(Divide)
        [Description("%")] Modulus,
        [Description("**")] Exponentiate,
        // Arithmetic Mutable
        [Description("++")] Increment,
        [Description("--")] Decrement,
        // Comparison
        [Description("==")] EqualTo,
        [Description("!=")] NotEqual,
        [Description(">")] GreaterThan,
        [Description("<")] LessThan,
        [Description(">=")] GreaterOrEqual,
        [Description("<=")] LessOrEqual,
        // Logical
        [Description("&&")] LAnd,
        [Description("||")] LOr,
        [Description("!")] LNot,
        // Bitwise
        [Description("&")] Ampersand, //(BAnd)
        [Description("|")] BOr,
        [Description("^")] BXor,
        [Description("<<")] BShiftLeft,
        [Description(">>")] BShiftRight,
        // Assignment
        [Description("=")] Set,
        [Description("+=")] AddSet,
        [Description("-=")] SubtractSet,
        [Description("*=")] MultiplySet,
        [Description("/=")] DivideSet,
        [Description("%=")] ModulusSet,
        [Description("&=")] BAndSet,
        [Description("|=")] BOrSet,
        [Description("^=")] BXorSet,
        [Description(">>=")] BShiftRightSet,
        [Description("<<=")] BShiftLeftSet,
        // Pointers and channels
        [Description("<-")] Receive,
        // Miscellaneous
        [Description("_")] Underscore,
        [Description(":=")] Assign,
        [Description(";")] Semicolon,
    }

    public static class OperatorExtension
    {
        private static readonly Dictionary<string, Operator> key_map = [];
        private static readonly Dictionary<Operator, string> value_map = [];
        private static readonly Operator[] values = Enum.GetValues<Operator>();

        static OperatorExtension() {
            const string descName = nameof(DescriptionAttribute);
            const string fieldName = nameof(FieldInfo);

            var type = typeof(Operator);

            foreach (var op in values)
            {
                var field = type.GetField(op.ToString())
                            ?? throw new NullReferenceException(fieldName);

                var desc = field.GetCustomAttribute<DescriptionAttribute>()
                           ?? throw new NullReferenceException(descName);

                var str = desc.Description;

                key_map.Add(str, op);
                value_map.Add(op, str);
            }
        }

        extension(Operator op)
        {
            public static Operator FromCode(string key)
            {
                return key_map.GetValueOrDefault(key);
            }

            public string? ToCode()
            {
                return value_map.GetValueOrDefault(op);
            }

            public static Operator[] Values()
            {
                return values;
            }
        }
    }

    public class MissingOperatorException : Exception;
}
