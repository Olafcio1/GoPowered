using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Statement;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial string HandleCode(List<IStatement> code);

        protected partial void HandleFunction(PTFunction func)
        {
            output += "\n";
            output += "func ";

            if (func.Parent != null)
            {
                HandleFuncParent(func);
                output += " ";
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

            output += " ";
            output += HandleCode(func.Body);
        }

        private void HandleFuncParent(PTFunction func)
        {
            output += "(";

            var name = func.Parent!.AssignName;
            if (name != null)
                output += name + " ";

            output += HandleType(func.Parent.Type);

            output += ")";
        }
    }
}
