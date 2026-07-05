using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Object;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial PTTypeInterface ParseTypeInterface(string name, Dictionary<string, IType>? generics)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            var Interface = new PTTypeInterface(name, [], [], generics);

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                if (!Now([("literal", null), (null, Operator.LParen.ToToken())], false))
                {
                    // Interface Inherit
                    Interface.Inherits.Add(Consume<LTLiteral>().Value);
                    continue;
                }

                var fName = Consume<LTLiteral>().Value;
                ParseFunctionSignature(out var fArgs, out var fReturns, out var fGenerics);
                Interface.Methods[fName] = new FunctionSignature(fName, fArgs, fReturns, fGenerics);
            }

            return Interface;
        }
    }
}
