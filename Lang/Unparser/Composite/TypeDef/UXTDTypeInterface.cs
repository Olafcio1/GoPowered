using GoPowered.Lang.Parser.Token.Object;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleTypeInterface(PTTypeInterface typeInterface)
        {
            output += "\n";
            output += "type ";
            output += typeInterface.Name;

            if (typeInterface.Generics != null)
                output += HandleFuncGenerics(typeInterface.Generics);

            output += " ";
            output += "interface {";
            output += HandleInherits(typeInterface.Inherits);
            output += HandleMethods(typeInterface.Methods);
            output += "\n}";
        }
    }
}
