using GoPowered.Lang.Parser.Token.Object;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleTypeGeneric(PTTypeGeneric typeGeneric)
        {
            output += "\n";
            output += "type ";
            output += typeGeneric.Name;

            if (typeGeneric.Generics != null)
                output += HandleFuncGenerics(typeGeneric.Generics);

            output += " ";
            output += "interface {\n    ";

            foreach (var type in typeGeneric.Types)
            {
                if (type.allowedInheritors)
                {
                    output += "~";
                }

                output += HandleType(type.Type);
                output += " ";
            }

            output += "\n}";
        }
    }
}
