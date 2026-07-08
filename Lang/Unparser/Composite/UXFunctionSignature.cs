using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleFuncGenerics(Dictionary<string, IType> generics)
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

        protected string HandleFuncArguments(List<Argument> args)
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

        protected string HandleFuncReturns(List<ReturnValue> returns)
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
    }
}
