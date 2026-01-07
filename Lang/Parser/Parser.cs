using GoPowered.Base;
using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Base;
using GoPowered.Lang.Parser.Type;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Parser
{
    public class Parser : BaseParser<ILexerToken, ParserError>
    {
        public readonly List<IParserToken> output;
        public string package;

        public Parser(List<ILexerToken> input)
             : base(input)
        {
            this.index = 0;
            this.output = [];
        }

        public void Parse()
        {
            Require(Keyword.PACKAGE.ToToken(), "'package'");
            package = Consume<LTLiteral>().Value;

            CollectImports();

            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.FUNCTION.ToToken())]))
                {
                    ParseFunction();
                }
            }
        }

        protected void ParseFunction()
        {
            Require(Keyword.FUNCTION.ToToken(), "'func'");

            var name = Consume<LTLiteral>().Value;
            var args = new List<Argument>();

            Require(Operator.LParen.ToToken(), "'('");

            var index = 0;
            while (true)
            {
                if (Now([(null, Operator.RParen.ToToken())], true))
                    break;
                else if (index++ != 0)
                    Require(Operator.Comma.ToToken(), "','");

                var aType = ParseType();
                var aName = Consume<LTLiteral>().Value;

                args.Add(new Argument(aName, aType));
            }

            var body = ParseCode();

            output.Add(new PTFunction(name, args, body));
        }

        protected IType ParseType()
        {
            if (Now([("literal", null)]))
            {
                var name = Consume<LTLiteral>().Value;
                return new UniqueType(name);
            } else if (Now([(null, Keyword.STRING.ToToken())], true))
            {
                return PrimitiveType.STRING;
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
            else if (Now([(null, Keyword.MAP.ToToken())], true))
            {
                Require(Operator.LSquare.ToToken(), "'['");
                var key = ParseType();
                Require(Operator.RSquare.ToToken(), "']'");

                var value = ParseType();
                return new MapType(key, value);
            }
            else if (Now([(null, Keyword.ERROR.ToToken())], true))
            {
                return PrimitiveType.ERROR;
            } else if (Now([(null, Operator.LSquare.ToToken())], true))
            {
                Require(Operator.RSquare.ToToken(), "']'");

                var type = ParseType();
                return new ListType(type);
            } else
            {
                throw new ParserError("Expected a type");
            }
        }

        protected List<IStatement> ParseCode()
        {
            Require(Operator.LCurly.ToToken(), "{");

            var code = new List<IStatement>();
            while (true)
            {
                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                code.Add(ParseStatement());
            }

            return code;
        }

        protected IStatement ParseStatement()
        {
            // TODO
        }

        protected void CollectImports()
        {
            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.IMPORT.ToToken())], true))
                {
                    output.Add(new PTImport(Consume<LTString>().Value));
                }
                else break;
            }
        }
    }
}
