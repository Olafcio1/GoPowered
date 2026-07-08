using GoPowered.Lang.Parser.Token;
using System.Text.Json;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial void HandleImportAll(PTImportAll importAll)
        {
            output += "\n";
            output += "import . ";
            output += JsonSerializer.Serialize(importAll.Package);
        }
    }
}
