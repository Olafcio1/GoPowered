using GoPowered.Lang.Parser.Token;
using System.Text.Json;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleImportAs(PTImportAs importAs)
        {
            output += "\n";
            output += "import ";
            output += importAs.Alias ?? "_";
            output += " ";
            output += JsonSerializer.Serialize(importAs.Package);
        }
    }
}
