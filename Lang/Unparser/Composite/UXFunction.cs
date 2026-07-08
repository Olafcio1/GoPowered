using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleFunction(PTFunction func)
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
                output += HandleFuncGenerics(generic);
            }

            output += HandleFuncArguments(func.Arguments);

            if (func.Returns != null)
            {
                output += " ";
                output += HandleFuncReturns(func.Returns);
            }

            output += ";";
        }
    }
}
