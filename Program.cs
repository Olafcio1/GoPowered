using GoPowered.Lang.Lexer;
using GoPowered.Lang.Parser;
using GoPowered.Lang.Unparser;
using GoPowered.PoweredLang.PoweredLexer;
using GoPowered.PoweredLang.PoweredParser;
using GoPowered.PoweredLang.PoweredUnparser;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace GoPowered {
    public class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length >= 1 && args[0] == "dump")
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: gopowered dump [filepath]");
                    return;
                }

                var path = args[1];

                if (!File.Exists(path))
                {
                    Console.WriteLine("Error: specified file not found");
                    return;
                }

                Console.Write(ProcessFile(path));
            }
            else if (args.Length >= 1 && args[0] == "run")
            {
                string path;

                if (args.Length < 2)
                    path = ".";
                else
                    path = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Error: specified directory not found");
                    return;
                }

                Compile(path);

                var proc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "go",
                    Arguments = "run ./.gopowered/compile/go/" + ((path.EndsWith(".go") || path.EndsWith(".gu")) ? path.Substring(0, path.Length - 3) + ".go" : path),
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });

                proc!.WaitForExit();
            }
            else if (args.Length >= 1 && args[0] == "build")
            {
                string path;

                if (args.Length < 2)
                    path = ".";
                else
                    path = args[1];

                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Error: specified directory not found");
                    return;
                }

                Compile(path);
            }
            else
            {
                Console.WriteLine("┌───────┤ GoPowered SDK ├───────┐");
                Console.WriteLine("│                               │");
                Console.WriteLine("│ ⏯️ run   - runs a workspace   │");
                Console.WriteLine("│ 🧱 build - builds a workspace │");
                Console.WriteLine("│ 🥟 dump  - dumps go code      │");
                Console.WriteLine("│                               │");
                Console.WriteLine("└───────────────────────────────┘");
            }
        }

        private static string ProcessFile(string path)
        {
            var input = File.ReadAllText(path)
                                .ReplaceLineEndings("\n");

            var lexer = new PoweredLexer(input);

            Parser parser;
            Unparser unparser;

            try
            {
                var lexed = lexer.Lex();

                parser = new PoweredParser(lexed).Parse();
                unparser = new PoweredUnparser(parser);

                var unparsed = unparser.Unparse();

                return unparsed;
            }
            catch (LexerError ex)
            {
                Console.WriteLine( "┌───────┤ GoPowered · Lexer Error ├───────┐");
                Console.WriteLine( "│                                         │");
                Console.WriteLine($"│ {new string(' ', (int)Math.Floor((39 - ex.Message.Length) / 2d))
                        + ex.Message
                        + new string(' ', (int)Math.Ceiling((39 - ex.Message.Length) / 2d))} │");

                Console.WriteLine( "│                                         │");

                var @event = $"line {lexer.lineno + 1}:{lexer.charno + 1}, file {path}";
                var divided = (39 - @event.Length) / 2d;

                Console.WriteLine($"│ {new string(' ', (int)Math.Floor(divided))
                        + @event
                        + new string(' ', (int)Math.Ceiling(divided))} │");
                Console.WriteLine( "│                                         │");
                Console.WriteLine( "└─────────────────────────────────────────┘");

                Console.WriteLine( " ⦆                                         ");
                Console.WriteLine($" ⦆ {input.Split("\n")[lexer.lineno].Trim()}");
                Console.WriteLine( " ⦆                                         ");

                Environment.Exit(1);

                return null;
            }
        }

        private static void Compile(string path, string sub = ".gopowered/compile/go/", bool master = true)
        {
            var files = Directory.EnumerateFileSystemEntries(path);
            string? toHide = null;

            if (!Directory.Exists(sub))
            {
                var currentFailing = sub;

                while (true)
                {
                    var value = Directory.GetParent(currentFailing);
                    if (value == null)
                        break;

                    if (value.Exists)
                        break;

                    currentFailing = value.FullName;
                }

                Directory.CreateDirectory(sub);

                toHide = currentFailing;
            }

            foreach (var rawfn in files)
            {
                var fn = rawfn;

                if (fn.Contains('/'))  fn = fn[(fn.LastIndexOf('/') + 1)..];
                if (fn.Contains('\\')) fn = fn[(fn.LastIndexOf('\\') + 1)..];

                if (fn.Equals(".gopowered"))
                    continue;

                var path2 = path + "/" + fn;

                if (Directory.Exists(path2))
                {
                    Compile(path2, sub + fn + "/", master: false);
                }
                else
                {
                    var unparsed = ProcessFile(path2);

                    File.WriteAllText(sub + fn.Substring(0, fn.LastIndexOf(".")) + ".go", unparsed);
                }
            }

            if (master && toHide != null)
            {
                var proc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "C:\\Windows\\System32\\attrib.exe",
                    Arguments = $"+h +i {toHide.Replace("/", "\\")} /D /S /L",
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });

                proc!.WaitForExit();
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
