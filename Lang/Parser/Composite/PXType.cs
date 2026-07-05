using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Type;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial IType? ParseType(bool optional = false)
        {
            if (Now([(null, Operator.LSquare.ToToken())], true))
            {
                if (Now([(null, Operator.RSquare.ToToken())], true))
                    return new ListType(ParseType()!);

                var amount = ParseExpression(allowLogic: false, constant: true);
                Require(Operator.RSquare.ToToken(), "']'");
                return new ArrayType(amount, ParseType()!);
            }
            else if (Now([("literal", null)]))
            {
                var location = new List<string>();

                do
                {
                    location.Add(Consume<LTLiteral>().Value);
                } while (Now([(null, Operator.Dot.ToToken())], true));

                var generics = ParseTypeGenerics();

                return new UniqueType(location, generics);
            }
            else if (Now([(null, Keyword.STRING.ToToken())], true))
            {
                return PrimitiveType.STRING;
            }
            else if (Now([(null, Keyword.RUNE.ToToken())], true))
            {
                return PrimitiveType.RUNE;
            }
            else if (Now([(null, Keyword.BOOL.ToToken())], true))
            {
                return PrimitiveType.BOOL;
            }
            else if (Now([(null, Keyword.INT.ToToken())], true))
            {
                return PrimitiveType.INT;
            }
            else if (Now([(null, Keyword.INT64.ToToken())], true))
            {
                return PrimitiveType.INT64;
            }
            else if (Now([(null, Keyword.INT32.ToToken())], true))
            {
                return PrimitiveType.INT32;
            }
            else if (Now([(null, Keyword.INT16.ToToken())], true))
            {
                return PrimitiveType.INT16;
            }
            else if (Now([(null, Keyword.INT8.ToToken())], true))
            {
                return PrimitiveType.INT8;
            }
            else if (Now([(null, Keyword.INT.ToToken())], true))
            {
                return PrimitiveType.INT;
            }
            else if (Now([(null, Keyword.UINT.ToToken())], true))
            {
                return PrimitiveType.UINT;
            }
            else if (Now([(null, Keyword.UINT64.ToToken())], true))
            {
                return PrimitiveType.UINT64;
            }
            else if (Now([(null, Keyword.UINT32.ToToken())], true))
            {
                return PrimitiveType.UINT32;
            }
            else if (Now([(null, Keyword.UINT16.ToToken())], true))
            {
                return PrimitiveType.UINT16;
            }
            else if (Now([(null, Keyword.UINT8.ToToken())], true))
            {
                return PrimitiveType.UINT8;
            }
            else if (Now([(null, Keyword.UINT.ToToken())], true))
            {
                return PrimitiveType.UINT;
            }
            else if (Now([(null, Keyword.FLOAT.ToToken())], true))
            {
                return PrimitiveType.FLOAT;
            }
            else if (Now([(null, Keyword.FLOAT64.ToToken())], true))
            {
                return PrimitiveType.FLOAT64;
            }
            else if (Now([(null, Keyword.FLOAT32.ToToken())], true))
            {
                return PrimitiveType.FLOAT32;
            }
            else if (Now([(null, Keyword.MAP.ToToken())], true))
            {
                Require(Operator.LSquare.ToToken(), "'['");
                var key = ParseType();
                Require(Operator.RSquare.ToToken(), "']'");

                var value = ParseType();
                return new MapType(key!, value!);
            }
            else if (Now([(null, Keyword.ERROR.ToToken())], true))
            {
                return PrimitiveType.ERROR;
            }
            else if (Now([(null, Keyword.FUNCTION.ToToken())], true))
            {
                ParseFunctionSignature(out var args, out var returns, out var generics);

                return new FunctionType(args, returns, generics);
            }
            else if (Now([(null, Keyword.STRUCT.ToToken())], true))
            {
                ParseTypeStruct(out var fields, out var inherits);

                return new StructType(fields, inherits);
            }
            else if (Now([(null, Keyword.INTERFACE.ToToken())], true))
            {
                ParseTypeInterface(out var methods, out var inherits);

                return new InterfaceType(methods, inherits);
            }
            else if (Now([(null, Operator.Star.ToToken())], true))
            {
                var type = ParseType();
                return new PointerType(type!);
            }
            else if (Now([(null, Keyword.ANY.ToToken())], true))
            {
                return AnyType.INSTANCE;
            }
            else if (optional)
            {
                return null;
            }
            else
            {
                //Console.WriteLine(Peek(-1));
                //Console.WriteLine(Peek(0));
                //Console.WriteLine(Peek(1));
                //Console.WriteLine(Peek(2));
                throw new ParserError("Expected a type");
            }
        }

        private List<IType>? ParseTypeGenerics()
        {
            List<IType>? generics = null;

            if (Now([(null, Operator.LSquare.ToToken())], true))
            {
                generics = [];

                var comma = false;

                while (true)
                {
                    if (Now([(null, Operator.RSquare.ToToken())], true))
                        break;
                    else if (!comma)
                        comma = true;
                    else Require(Operator.Comma.ToToken(), "','");

                    generics.Add(ParseType()!);
                }
            }

            return generics;
        }
    }
}
