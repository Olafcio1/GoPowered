using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected partial bool HandleMember(IExpressionPart part, ref string output)
        {
            if (part is EPMember member)
            {
                output += ".";
                output += member.Name;

                return true;
            }

            return false;
        }
    }
}
