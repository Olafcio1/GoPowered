using System.ComponentModel;
using System.Reflection;

namespace GoPowered.Lang.Lexer.Token
{
    public record LTOperator(Operator Value) : ILexerToken
    {
        public string Type() => "operator";
    }

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
        [Description(">=")] GreaterOrEqual,
        [Description("<=")] LessOrEqual,
        [Description(">")] GreaterThan,
        [Description("<")] LessThan,
        // Logical
        [Description("&&")] LAnd,
        [Description("||")] LOr,
        [Description("!")] LNot,
        // Bitwise
        [Description("&")] Ampersand,
        [Description("|")] VLine,
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
        [Description("<-")] Transmit,
        // Miscellaneous
        //[Description("_")] Underscore,  //-- checking temporary moved to the parser, TODO make a special case or a custom token for it
        [Description(":=")] Assign,
        [Description(":")] Colon,
        [Description(";")] Semicolon,
        [Description(",")] Comma,
        [Description("...")] Ellipsis,
        [Description(".")] Dot,
        [Description("~")] Tilde
    }

    public static class OperatorExtension
    {
        private static readonly Dictionary<string, Operator> key_map = [];
        private static readonly Dictionary<Operator, string> value_map = [];
        private static readonly Dictionary<Operator, LTOperator> token_map = [];

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
                token_map.Add(op, new LTOperator(op));
            }

            values.Sort(static (a, b) => b.ToCode()!.Length.CompareTo(a.ToCode()!.Length));
        }

        extension(Operator op)
        {
            [Unstable]
            public static Operator FromCode(string key)
            {
                return key_map.GetValueOrDefault(key);
            }

            public string? ToCode()
            {
                return value_map.GetValueOrDefault(op);
            }

            public LTOperator ToToken()
            {
                return token_map.GetValueOrDefault(op)
                       ?? throw new MissingOperatorException();
            }

            [Unstable]
            public static Operator[] Values()
            {
                return values;
            }
        }
    }

    [Unstable]
    public class MissingOperatorException : Exception;
}
