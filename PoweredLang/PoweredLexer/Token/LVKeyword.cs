using GoPowered.Lang.Lexer;
using System.ComponentModel;
using System.Reflection;

namespace GoPowered.PoweredLang.PoweredLexer.Token
{
    public record LVKeyword : ILexerToken
    {
        public VKeyword Value { get; }
        internal LVKeyword(VKeyword Value) {
            this.Value = Value;
        }

        public string Type() => "keyword";
    }

    public enum VKeyword
    {
        // Datatype Definition
        [Description("object")] OBJECT,

        // Variable Definition
        [Description("final")] FINAL,
    }

    public static class VKeywordExtension
    {
        private static readonly Dictionary<string, VKeyword> key_map = [];
        private static readonly Dictionary<VKeyword, string> value_map = [];
        private static readonly Dictionary<VKeyword, LVKeyword> token_map = [];

        private static readonly VKeyword[] values = Enum.GetValues<VKeyword>();

        static VKeywordExtension()
        {
            const string descName = nameof(DescriptionAttribute);
            const string fieldName = nameof(FieldInfo);

            var type = typeof(VKeyword);

            foreach (var kw in values)
            {
                var field = type.GetField(kw.ToString())
                            ?? throw new NullReferenceException(fieldName);

                var desc = field.GetCustomAttribute<DescriptionAttribute>()
                           ?? throw new NullReferenceException(descName);

                var str = desc.Description;

                key_map.Add(str, kw);
                value_map.Add(kw, str);
                token_map.Add(kw, new LVKeyword(kw));
            }
        }

        extension(VKeyword kw)
        {
            [Unstable]
            public static bool FromCode(string key, out VKeyword value)
            {
                return key_map.TryGetValue(key, out value);
            }

            [Unstable]
            public static VKeyword FromCode(string key)
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

            public LVKeyword ToToken()
            {
                return token_map.GetValueOrDefault(kw)
                       ?? throw new MissingKeywordException();
            }

            [Unstable]
            public static VKeyword[] Values()
            {
                return values;
            }
        }
    }

    [Unstable]
    public class MissingKeywordException : Exception;
}
