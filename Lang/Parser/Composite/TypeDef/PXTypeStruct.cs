using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial void ParseTypeStruct(out Dictionary<string, IType> Fields, out List<string> Inherits)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            Fields = [];
            Inherits = [];

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                if (Now([("literal", null), ("newline", null)], false))
                {
                    // Struct Inherit
                    Inherits.Add(Consume<LTLiteral>().Value);
                    continue;
                }

                var fNames = new List<string>();

                do
                {
                    fNames.Add(Consume<LTLiteral>().Value);
                } while (Now([(null, Operator.Comma.ToToken())], true));

                var fType = ParseType();

                foreach (var fName in fNames)
                    Fields[fName] = fType!;

                Require(LTNewLine.INSTANCE, "newline");
            }
        }
    }
}
