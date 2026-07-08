using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Object;
using GoPowered.Lang.Parser.Token.Object.Section;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
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
                    HandleFunction(func);
                }
                else if (tok is PTTypeAlias typeAlias)
                {
                    HandleTypeAlias(typeAlias);
                }
                else if (tok is PTTypeClone typeClone)
                {
                    HandleTypeClone(typeClone);
                }
                else if (tok is PTTypeStruct typeStruct)
                {
                    HandleTypeStruct(typeStruct);
                }
                else if (tok is PTTypeInterface typeInterface)
                {
                    HandleTypeInterface(typeInterface);
                }
                else if (tok is PTTypeGeneric typeGeneric)
                {
                    HandleTypeGeneric(typeGeneric);
                }
                else if (tok is PTImport import)
                {
                    HandleImport(import);
                }
            }

            return output;
        }

        protected partial void HandleFunction(PTFunction func);

        protected partial void HandleTypeAlias(PTTypeAlias typeAlias);

        protected partial void HandleTypeClone(PTTypeClone typeClone);

        protected partial void HandleTypeStruct(PTTypeStruct typeStruct);

        protected partial void HandleTypeInterface(PTTypeInterface typeInterface);

        protected partial void HandleTypeGeneric(PTTypeGeneric typeGeneric);

        protected partial string HandleType(IType type);

        protected partial void HandleImport(PTImport import);

        protected string HandleFunctionSignature(FunctionSignature func)
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
