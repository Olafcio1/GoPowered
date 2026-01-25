using GoPowered.Lang.Lexer;
using GoPowered.Lang.Parser;
using System.Collections;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace GoPowered {
    public class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length == 2 && args[0] == "run") {
                var path = args[1];

                var lexed = new Lexer(File.ReadAllText(path)).Lex();
                var parser = new Parser(lexed);
                parser.Parse();

                Console.Write(lexed.Count);
                Console.Write(Repr(parser.output));
            } else {
                Console.WriteLine("┌───────┤ GoPowered SDK ├───────┐");
                Console.WriteLine("│                               │");
                Console.WriteLine("│ ⏯️ run   - runs a workspace   │");
                Console.WriteLine("│ 🧱 build - builds a workspace │");
                Console.WriteLine("│                               │");
                Console.WriteLine("└───────────────────────────────┘");
            }
        }

        static string Repr(object? value, string nl = "\n", string tab1 = "", string tab2 = "  ", string objAdditional = "")
        {
            if (value is string || value is int || value is short || value is double || value is long || value is float || value is bool)
            {
                return "" + value;
            }
            else if (value == null)
            {
                return "null";
            }
            else if (value is IEnumerable array)
            {
                var repr = "[";

                foreach (var val in array)
                    repr += (nl + tab2 + Repr(val, tab1: tab1, tab2: tab2 + "  "));

                repr += nl + tab1;
                repr += "]";

                return repr;
            } else
            {
                var type = value.GetType();
                var properties = type.GetProperties();

                var tab1x = tab1;
                var tab2x = tab2;

                if (properties.Any(prop => prop.GetValue(value) is IEnumerable))
                    tab1x += "  ";

                var repr = type.Name + objAdditional + "{";
                var index = 0;
                foreach (var f in properties)
                {
                    var val = f.GetValue(value);

                    if (val == null)
                    {
                        repr += "null";
                    }
                    else if (f.Name != val.GetType().Name)
                    {
                        repr += (f.Name);
                        repr += ("=");
                        repr += Repr(val, tab1: tab1x, tab2: tab2x);
                    }
                    else
                    {
                        repr += Repr(val, tab1: tab1x, tab2: tab2x, objAdditional: "=");
                    }

                    if (++index != properties.Length)
                        repr += (", ");
                }

                repr += ("}");
                return repr;
            }
        }
    }
}
