using GoPowered.Lang.Parser.Token;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleTypeAlias(PTTypeAlias typeAlias)
        {
            output += "\n";
            output += "type ";

            output += typeAlias.Name;
            output += " = ";
            output += HandleType(typeAlias.Type);
        }
    }
}
