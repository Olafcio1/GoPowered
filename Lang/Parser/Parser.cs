using GoPowered.Base;
using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Expr.Target;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Type;
using GoPowered.Lang.Parser.Type.Go;

namespace GoPowered.Lang.Parser
{
    public class Parser : BaseParser<ILexerToken, ParserError>
    {
        public readonly List<IParserToken> output;
        public string? package;

        public Parser(List<ILexerToken> input)
             : base(input)
        {
            this.index = 0;
            this.output = [];
        }

        protected override string TypeOf(ILexerToken token)
        {
            return token.Type();
        }

        public void Parse()
        {
            ConsumeNewlines();

            Require(Keyword.PACKAGE.ToToken(), "'package'");
            package = Consume<LTLiteral>().Value;

            CollectImports();

            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.FUNCTION.ToToken())]))
                    ParseFunction();
                else if (Now([("newline", null)], true))
                    continue;
                else
                    throw new ParserError("Expected a global statement");
            }
        }

        protected void ConsumeNewlines()
        {
            while (Now([("newline", null)]))
                Consume();
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

                var aName = Consume<LTLiteral>().Value;
                var aType = ParseType();

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
                else if (Now([("newline", null)], true))
                    continue;

                code.Add(ParseStatement());
            }

            return code;
        }

        protected IStatement ParseStatement()
        {
            var expr = ParseExpression();
            return new StmtExpression(expr);
        }

        protected Expression ParseExpression()
        {
            if (Now([("string", null)], false))
                return new Expression(new ESTString(Consume<LTString>().Value), null, singular: true);
            else if (Now([("integer", null)], false))
                return new Expression(new ESTInteger(Consume<LTInteger>().Value), null, singular: true);
            else if (Now([("float", null)], false))
                return new Expression(new ESTFloat(Consume<LTFloat>().Value), null, singular: true);
            else return ParsePartExpression();
        }

        /**
         * I also like to call it the `friendly expression`, as it's friendly to [expression] parts
         */
        protected Expression ParsePartExpression()
        {
            var target = ParseExpressionTarget();
            var parts = new List<IExpressionPart>();

            while (true)
            {
                if (Now([(null, Operator.Dot.ToToken())], true))
                {
                    parts.Add(new EPMember(Consume<LTLiteral>().Value));
                }
                else if (Now([(null, Operator.LSquare.ToToken())], true))
                {
                    parts.Add(new EPAccess(ParseExpression()));
                    Require(Operator.RSquare.ToToken(), "']'");
                }
                else if (Now([(null, Operator.LParen.ToToken())], true))
                {
                    var args = new List<Expression>();
                    var comma = false;

                    while (true)
                    {
                        if (Now([(null, Operator.RParen.ToToken())], true))
                            break;
                        else if (comma)
                            Require(Operator.Comma.ToToken(), "','");
                        else comma = true;

                        args.Add(ParseExpression());
                    }

                    parts.Add(new EPCall(args));
                } else
                {
                    break;
                }
            }

            return new Expression(
                target,
                parts
            );
        }

        protected IExpressionTarget ParseExpressionTarget()
        {
            if (Now([("literal", null)], false))
            {
                var literal = Consume<LTLiteral>().Value;
                return new ETReference(literal);
            } else
            {
                // Should this error message be 'Expected an expression' instead?
                throw new ParserError("Expected expression target");
            }
        }

        protected void CollectImports()
        {
            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.IMPORT.ToToken())], true))
                    output.Add(new PTImport(Consume<LTString>().Value));
                else if (Now([("newline", null)], true))
                    continue;
                else break;
            }
        }
    }
}
