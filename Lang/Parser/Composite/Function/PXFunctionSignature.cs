using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial void ParseFunctionSignature(out List<Argument> args, out List<ReturnValue>? returns, out Dictionary<string, IType>? generics)
        {
            args = new List<Argument>();
            generics = ParseDefGenerics();

            Require(Operator.LParen.ToToken(), "'('");

            int ind = this.index;

            try                 {                   ParseUnnamedArguments(args); }
            catch (ParserError) { this.index = ind; ParseNamedArguments(args);   }

            returns = ParseReturnTypes();
        }

        private void ParseNamedArguments(List<Argument> args)
        {
            var index = 0;

            while (true)
            {
                if (Now([(null, Operator.RParen.ToToken())], true))
                    break;
                else if (index++ != 0)
                    Require(Operator.Comma.ToToken(), "','");

                var aName = Consume<LTLiteral>().Value;
                var ellipsis = Now([(null, Operator.Ellipsis.ToToken())], true);
                var aType = ParseType(true);

                if (ellipsis && aType == null)
                    throw new ParserError("A rest argument must have a type");

                if (aType != null)
                    foreach (var arg in args)
                        if (arg.Type == null)
                            arg.Type = aType;

                args.Add(new Argument(
                    aName,
                    aType!,
                    ellipsis
                ));
            }

            foreach (var arg in args)
                if (arg.Type == null)
                    throw new ParserError("Missing inherited parameter type");
        }

        private void ParseUnnamedArguments(List<Argument> args)
        {
            var index = 0;

            while (true)
            {
                if (Now([(null, Operator.RParen.ToToken())], true))
                    break;
                else if (index++ != 0)
                    Require(Operator.Comma.ToToken(), "','");

                var ellipsis = Now([(null, Operator.Ellipsis.ToToken())], true);
                var aType = ParseType()!;

                args.Add(new Argument(
                    null,
                    aType,
                    ellipsis
                ));
            }
        }

        private List<ReturnValue>? ParseReturnTypes()
        {
            List<ReturnValue> returns = [];

            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                do
                {
                    string? rName = null;
                    if (Peek(0).Type() == "literal" && !Peek(1).Equals(Operator.Comma.ToToken()) && !Peek(1).Equals(Operator.RParen.ToToken()))
                        rName = Consume<LTLiteral>().Value;

                    var rType = ParseType();
                    returns.Add(new ReturnValue(
                        rName,
                        rType!
                    ));
                } while (Now([(null, Operator.Comma.ToToken())], true));

                Require(Operator.RParen.ToToken(), ")");
            }
            else
            {
                var type = ParseType(true);
                if (type == null)
                    return null;

                returns.Add(new ReturnValue(
                    null,
                    type!
                ));
            }

            return returns;
        }
    }
}
