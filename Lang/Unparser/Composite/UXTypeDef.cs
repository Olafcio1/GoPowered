using GoPowered.Lang.Parser.Token.Object.Section;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleInherits(List<string> inherits)
        {
            var value = "";

            foreach (var name in inherits)
                value += "\n    " + name;

            return value;
        }

        protected string HandleFields(Dictionary<string, IType> fields)
        {
            var value = "";

            foreach (var (name, type) in fields)
                value += "\n    " + name + "    " + HandleType(type);

            return value;
        }

        protected string HandleMethods(Dictionary<string, FunctionSignature> methods)
        {
            var value = "";

            foreach (var (name, type) in methods)
                value += "\n    " + name + "    " + HandleFunctionSignature(type);

            return value;
        }
    }
}
