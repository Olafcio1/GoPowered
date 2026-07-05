using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial PTFunction ParseFunctionDef()
        {
            Require(Keyword.FUNCTION.ToToken(), "'func'");

            MethodParent? parent = null;

            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var assign = (Peek(0) is LTLiteral && (Peek(1) is not LTOperator op || op.Value != Operator.RParen))
                                ? Consume<LTLiteral>().Value
                                : null;

                var type = ParseType();

                if (type is PointerType pointer)
                {
                    if (pointer.Type is not UniqueType)
                        throw new ParserError("A method's parent must be a struct or a pointer to a struct");
                }
                else if (type is not UniqueType)
                {
                    throw new ParserError("A method's parent must be a struct or a pointer to a struct");
                }

                parent = new MethodParent(assign, type);

                Require(Operator.RParen.ToToken(), "')'");
            }

            var name = Consume<LTLiteral>().Value;
            ParseFunctionSignature(out var args, out var returns, out var generics);

            var body = ParseCode();
            return new PTFunction(name, args, body, returns, parent, generics);
        }
    }
}
