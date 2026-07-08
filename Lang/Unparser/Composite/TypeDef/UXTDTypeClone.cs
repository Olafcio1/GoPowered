using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleTypeClone(PTTypeClone typeClone)
        {
            output += "\n";
            output += "type ";

            output += typeClone.Name;

            if (typeClone.Generics != null)
                output += HandleFuncGenerics(typeClone.Generics);

            output += " ";
            output += HandleType(typeClone.Type);
        }
    }
}
