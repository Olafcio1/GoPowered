using System.ComponentModel;
using System.Reflection;

namespace GoPowered.Lang.Lexer.Token
{
    public record LTKeyword : ILexerToken
    {
        public Keyword Value { get; }
        internal LTKeyword(Keyword Value) {
            this.Value = Value;
        }

        public string Type() => "keyword";
    }

    public enum Keyword
    {
        [Description("package")] PACKAGE,
        [Description("import")] IMPORT,
        [Description("func")] FUNCTION,
        [Description("chan")] CHANNEL,
        [Description("goto")] GOTO,
        // Function Invocations
        [Description("go")] GO,
        [Description("defer")] DEFER,
        [Description("return")] RETURN,
        // If
        [Description("if")] IF,
        [Description("else")] ELSE,
        // For
        [Description("for")] FOR,
        [Description("range")] RANGE,
        [Description("continue")] CONTINUE,
        [Description("break")] BREAK,
        // Datatype Definition
        [Description("type")] TYPE,
        [Description("struct")] STRUCT,
        [Description("interface")] INTERFACE,
        // Variable Definition
        [Description("var")] VARIABLE,
        [Description("const")] CONST,
        // Switch / Select
        [Description("switch")] SWITCH,
        [Description("select")] SELECT,
        [Description("case")] CASE,
        [Description("default")] DEFAULT,
        [Description("fallthrough")] FALLTHROUGH,
        // Special Functions
        [Description("panic")] PANIC,
        [Description("make")] MAKE,
        // Special Values
        [Description("nil")] NIL,
        [Description("true")] TRUE,
        [Description("false")] FALSE,
        // Built-in Types
        [Description("string")] STRING,
        [Description("int")] INT,
        [Description("int64")] INT64,
        [Description("int32")] INT32,
        [Description("int16")] INT16,
        [Description("int8")] INT8,
        [Description("uint")] UINT,
        [Description("uint64")] UINT64,
        [Description("uint32")] UINT32,
        [Description("uint16")] UINT16,
        [Description("uint8")] UINT8,
        [Description("float")] FLOAT,
        [Description("map")] MAP,
        [Description("error")] ERROR,
    }

    public static class KeywordExtension
    {
        private static readonly Dictionary<string, Keyword> key_map = [];
        private static readonly Dictionary<Keyword, string> value_map = [];
        private static readonly Dictionary<Keyword, LTKeyword> token_map = [];

        private static readonly Keyword[] values = Enum.GetValues<Keyword>();

        static KeywordExtension()
        {
            const string descName = nameof(DescriptionAttribute);
            const string fieldName = nameof(FieldInfo);

            var type = typeof(Keyword);

            foreach (var kw in values)
            {
                var field = type.GetField(kw.ToString())
                            ?? throw new NullReferenceException(fieldName);

                var desc = field.GetCustomAttribute<DescriptionAttribute>()
                           ?? throw new NullReferenceException(descName);

                var str = desc.Description;

                key_map.Add(str, kw);
                value_map.Add(kw, str);
                token_map.Add(kw, new LTKeyword(kw));
            }
        }

        extension(Keyword kw)
        {
            [Unstable]
            public static bool FromCode(string key, out Keyword value)
            {
                return key_map.TryGetValue(key, out value);
            }

            [Unstable]
            public static Keyword FromCode(string key)
            {
                if (!FromCode(key, out var value))
                    throw new MissingKeywordException();

                return value;
            }

            [Unstable]
            public string? ToCode()
            {
                return value_map.GetValueOrDefault(kw);
            }

            public LTKeyword ToToken()
            {
                return token_map.GetValueOrDefault(kw)
                       ?? throw new MissingKeywordException();
            }

            [Unstable]
            public static Keyword[] Values()
            {
                return values;
            }
        }
    }

    [Unstable]
    public class MissingKeywordException : Exception;
}
