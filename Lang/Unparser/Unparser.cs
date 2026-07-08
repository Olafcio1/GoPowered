using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Object.Section;
using GoPowered.Lang.Parser.Type;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Unparser
{
    public class Unparser
    {
        public readonly Parser.Parser input;
        public string output;

        public Unparser(Parser.Parser input) {
            this.input = input;
            this.output = "";
        }

        //protected override string TypeOf(IParserToken token)
        //{
        //    var text = token.GetType().Name.Substring(2);
        //    var dashed = "";

        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        var ch = text[i];

        //        if (ch >= 'A' && ch <= 'Z')
        //        {
        //            ch = Char.ToLower(ch);

        //            if (i > 0)
        //                dashed += "-";
        //        }

        //        dashed += ch;
        //    }

        //    return dashed;
        //}

        public string Unparse()
        {
            output += "/**\n";
            output += " * Code compiled from GoPowered\n";
            output += " * \n";
            output += " * more info at:\n";
            output += " * - https://olafcio1.github.io/?utm_source=GoPowered\n";
            output += " */\n";
            output += "package " + input.package + "\n";

            foreach (var tok in input.output)
            {
                if (tok is PTFunction func)
                {
                    output += "\n";
                    output += "func ";

                    if (func.Parent != null)
                    {
                        output += "(";

                        var name = func.Parent.AssignName;
                        if (name != null)
                            output += name + " ";

                        output += HandleType(func.Parent.Type);
                        output += ") ";
                    }

                    output += func.Name;

                    var generic = func.Generics;
                    if (generic != null)
                    {
                        output += "[";

                        bool gfirst = true;

                        foreach (var (key, val) in generic)
                        {
                            if (gfirst)
                                gfirst = false;
                            else
                                output += ", ";

                            output += key;
                            output += " ";
                            output += HandleType(val);
                        }

                        output += "]";
                    }

                    output += "(";

                    var args = func.Arguments;
                    var first = true;

                    foreach (var arg in args)
                    {
                        if (first)
                            first = false;
                        else
                            output += ", ";

                        if (arg.Name != null)
                        {
                            output += arg.Name;
                            output += " ";
                        }

                        output += HandleType(arg.Type);
                    }

                    output += ")";

                    if (func.Returns != null)
                    {
                        output += " ";
                        output += HandleFuncReturns(func.Returns);
                    }

                    output += ";";
                }
            }

            return output;
        }

        protected static string HandleType(IType type)
        {
            if (type is AnyType)
                return "any";
            else if (type is ArrayType)
                return "---";
            else if (type is ChannelType chan)
                return "chan " + HandleType(chan.Type);
            else if (type is FunctionType func)
                return "func" + (func.Generics == null
                                    ? null
                                    : HandleFuncGenerics(func.Generics))
                              + HandleFuncArguments(func.Args)
                              + (func.Returns == null
                                    ? null
                                    : HandleFuncReturns(func.Returns));
            else if (type is InterfaceType @interface)
                return "interface{" + HandleInherits(@interface.Inherits)
                                    + HandleMethods(@interface.Methods)
                                    + "\n}";
            else if (type is ListType list)
                return "[]" + HandleType(list.ElementType);
            else if (type is MapType map)
                return "map[" + HandleType(map.Key) + "]" + HandleType(map.Value);
            else if (type is PointerType ptr)
                return "*" + HandleType(ptr.Type);
            else if (type is PrimitiveType prim)
                return prim.Primitive.ToString().ToLower();
            else if (type is StructType @struct)
                return "struct{" + HandleInherits(@struct.Inherits)
                                 + HandleFields(@struct.Fields) + "\n}";
            else if (type is UniqueType uniq)
                return string.Join('.', uniq.Location) + (uniq.Generics == null
                                                            ? ""
                                                            : HandleTypeGenerics(uniq.Generics));
            else throw new UnparserError("Unrecognized type '" + type + "'");
        }

        protected static string HandleTypeGenerics(List<IType> generics)
        {
            var value = "[";
            var first = true;

            foreach (var type in generics)
            {
                if (first)
                    first = false;
                else value += ", ";

                value += HandleType(type);
            }

            value += "]";

            return value;
        }

        protected static string HandleFuncGenerics(Dictionary<string, IType> generics)
        {
            var value = "[";
            var first = true;

            foreach (var (key, val) in generics)
            {
                if (first)
                    first = false;
                else value += ", ";

                value += key;
                value += " ";
                value += HandleType(val);
            }

            value += "]";

            return value;
        }

        protected static string HandleFuncArguments(List<Argument> args)
        {
            var value = "(";
            var first = true;

            foreach (var arg in args)
            {
                if (first)
                    first = false;
                else value += ", ";

                if (arg.Name != null)
                {
                    value += arg.Name;
                    value += " ";
                }

                value += HandleType(arg.Type);
            }

            value += ")";

            return value;
        }

        protected static string HandleFuncReturns(List<ReturnValue> returns)
        {
            if (returns.Count == 1 && returns[0].Name == null)
                return HandleType(returns[0].Type);

            var value = "(";
            var first = true;

            foreach (var ret in returns)
            {
                if (first)
                    first = false;
                else value += ", ";

                if (ret.Name != null)
                {
                    value += ret.Name;
                    value += " ";
                }

                value += HandleType(ret.Type);
            }

            value += ")";

            return value;
        }

        protected static string HandleInherits(List<string> inherits)
        {
            var value = "";

            foreach (var name in inherits)
                value += "\n    " + name;

            return value;
        }

        protected static string HandleFields(Dictionary<string, IType> fields)
        {
            var value = "";

            foreach (var (name, type) in fields)
                value += "\n    " + name + "    " + HandleType(type);

            return value;
        }

        protected static string HandleMethods(Dictionary<string, FunctionSignature> methods)
        {
            var value = "";

            foreach (var (name, type) in methods)
                value += "\n    " + name + "    " + HandleFunctionSignature(type);

            return value;
        }

        protected static string HandleFunctionSignature(FunctionSignature func)
        {
            return "func" + (func.Generics == null
                    ? null
                    : HandleFuncGenerics(func.Generics))
              + HandleFuncArguments(func.Args)
              + (func.Returns == null
                    ? null
                    : HandleFuncReturns(func.Returns));
        }
    }
}
