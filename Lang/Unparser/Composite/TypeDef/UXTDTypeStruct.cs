using GoPowered.Lang.Parser.Token.Object;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleTypeStruct(PTTypeStruct typeStruct)
        {
            output += "\n";
            output += "type ";
            output += typeStruct.Name;

            if (typeStruct.Generics != null)
                output += HandleFuncGenerics(typeStruct.Generics);

            output += " ";
            output += "struct {";
            output += HandleInherits(typeStruct.Inherits);
            output += HandleFields(typeStruct.Fields);
            output += "\n}";
        }
    }
}
