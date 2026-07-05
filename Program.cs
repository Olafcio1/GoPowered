using GoPowered.PoweredLang.PoweredLexer;
using GoPowered.PoweredLang.PoweredParser;
using System.Collections;
using System.Text;

namespace GoPowered {
    public class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length == 2 && args[0] == "run") {
                var path = args[1];

                var lexed = new PoweredLexer(File.ReadAllText(path)).Lex();
                var parser = new PoweredParser(lexed);
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
            if (value is string || value is char || value is int || value is short || value is double || value is long || value is float || value is bool)
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
                var fields = type.GetFields();
                var properties = type.GetProperties();

                var allValues = new List<TypeValue>();
                if (properties.Length == 0)
                {
                    if (value is not Enum && value is not IAvoidSerialization)
                        foreach (var f in fields)
                            if (f.GetType() != type)
                                allValues.Add(new TypeValue(f.Name, f.GetValue(value)));

                    if (allValues.Count == 0)
                        return type.Name + objAdditional + "(" + value.ToString() + ")";
                }
                else
                    foreach (var f in properties)
                        allValues.Add(new TypeValue(f.Name, f.GetValue(value)));

                var tab1x = tab1;
                var tab2x = tab2;

                if (allValues.Any(prop => prop.Value is IEnumerable))
                    tab1x += "  ";

                var repr = type.Name + objAdditional + "{";
                var index = 0;
                foreach (var f in allValues)
                {
                    var val = f.Value;

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

                    if (++index != allValues.Count)
                        repr += (", ");
                }

                repr += ("}");
                return repr;
            }
        }
    }

    internal record TypeValue(string Name, object? Value);
}
