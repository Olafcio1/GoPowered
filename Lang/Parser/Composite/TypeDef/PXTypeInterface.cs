using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Object.Section;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial void ParseTypeInterface(out Dictionary<string, FunctionSignature> Methods, out List<string> Inherits)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            Inherits = [];
            Methods = [];

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                if (!Now([("literal", null), (null, Operator.LParen.ToToken())], false))
                {
                    // Interface Inherit
                    Inherits.Add(Consume<LTLiteral>().Value);
                    continue;
                }

                var fName = Consume<LTLiteral>().Value;
                ParseFunctionSignature(out var fArgs, out var fReturns, out var fGenerics);
                Methods[fName] = new FunctionSignature(fName, fArgs, fReturns, fGenerics);
            }
        }
    }
}
