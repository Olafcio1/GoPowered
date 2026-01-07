using GoPowered.Lang.Lexer;
using GoPowered.Lang.Parser;
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
                Console.WriteLine("[");
                foreach (var token in parser.output)
                {
                    var type = token.GetType();
                    var properties = type.GetProperties();

                    Console.Write("  " + type.Name + "{");
                    foreach (var f in properties)
                    {
                        Console.Write(f.Name);
                        Console.Write("=");
                        Console.Write(f.GetValue(token));
                    }
                    Console.Write("}\n");
                }
                Console.WriteLine("]");
            } else {
                Console.WriteLine("┌───────┤ GoPowered SDK ├───────┐");
                Console.WriteLine("│                               │");
                Console.WriteLine("│ ⏯️ run   - runs a workspace   │");
                Console.WriteLine("│ 🧱 build - builds a workspace │");
                Console.WriteLine("│                               │");
                Console.WriteLine("└───────────────────────────────┘");
            }
        }
    }
}
