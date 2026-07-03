using GoPowered.Base;
using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Expr.Target;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.Object;
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
                    output.Add(ParseFunctionDef());
                else if (Now([("newline", null)], true))
                    continue;
                else if (Now([(null, Keyword.TYPE.ToToken())]))
                    output.Add(ParseTypeDef());
                else
                    throw new ParserError("Expected a global statement");
            }
        }

        protected void ConsumeNewlines()
        {
            while (Now([("newline", null)]))
                Consume();
        }

        protected PTFunction ParseFunctionDef()
        {
            Require(Keyword.FUNCTION.ToToken(), "'func'");

            ParseFunctionSignature(out var name, out var args, out var returns);

            var body = ParseCode();
            return new PTFunction(name, args, body, returns);
        }

        protected void ParseFunctionSignature(out string name, out List<Argument> args, out List<ReturnValue>? returns)
        {
            name = Consume<LTLiteral>().Value;
            args = new List<Argument>();

            Require(Operator.LParen.ToToken(), "'('");

            var index = 0;
            while (true)
            {
                if (Now([(null, Operator.RParen.ToToken())], true))
                    break;
                else if (index++ != 0)
                    Require(Operator.Comma.ToToken(), "','");

                var aName = Consume<LTLiteral>().Value;
                var aType = ParseType(true);

                if (aType != null)
                    foreach (var arg in args)
                        if (arg.Type == null)
                            arg.Type = aType;

                args.Add(new Argument(
                    aName,
                    aType!
                ));
            }

            foreach (var arg in args)
                if (arg.Type == null)
                    throw new ParserError("Missing inherited parameter type");

            returns = null;

            if (!Now([(null, Operator.LCurly.ToToken())], false))
            {
                returns = [];

                if (Now([(null, Operator.LParen.ToToken())], true))
                {
                    do
                    {
                        string? rName = null;
                        if (Peek(0).Type() == "literal" && !Peek(1).Equals(Operator.RParen.ToToken()))
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
                    returns.Add(new ReturnValue(
                        null,
                        ParseType()!
                    ));
                }
            }
        }

        protected IParserToken ParseTypeDef()
        {
            Require(Keyword.TYPE.ToToken(), "'type'");

            var name = Consume<LTLiteral>().Value;

            if (Now([(null, Keyword.STRUCT.ToToken())], true))
                return ParseTypeStruct(name);
            else if (Now([(null, Keyword.INTERFACE.ToToken())], true))
                return ParseTypeInterface(name);
            else if (ParseType_out(out IType? type, optional: true))
                return new PTTypeClone(name, type!);
            else if (Now([(null, Operator.Set.ToToken())], true))
                return new PTTypeAlias(name, ParseType()!);
            else throw new ParserError("Expected a struct, interface, type alias or type clone");
        }

        protected IParserToken ParseTypeStruct(string name)
        {
            // TODO
            throw new ParserError("Not Implemented Yet");
        }

        protected PTTypeInterface ParseTypeInterface(string name)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            var Interface = new PTTypeInterface(name, [], []);

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

                ParseFunctionSignature(out var fName, out var fArgs, out var fReturns);
                Interface.Methods.Add(new FunctionSignature(fName, fArgs, fReturns));
            }

            return Interface;
        }

        protected bool ParseType_out(out IType? type, bool optional)
        {
            type = ParseType(optional);
            return type != null;
        }

        protected IType? ParseType(bool optional = false)
        {
            if (Now([("literal", null)]))
            {
                var name = Consume<LTLiteral>().Value;
                return new UniqueType(name);
            }
            else if (Now([(null, Keyword.STRING.ToToken())], true))
            {
                return PrimitiveType.STRING;
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
            else if (Now([(null, Operator.LSquare.ToToken())], true))
            {
                Require(Operator.RSquare.ToToken(), "']'");

                var type = ParseType();
                return new ListType(type!);
            }
            else if (Now([(null, Operator.Star.ToToken())], true))
            {
                var type = ParseType();
                return new PointerType(type!);
            }
            else if (optional)
            {
                return null;
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
                //Console.WriteLine(Peek(0));
                Require(LTNewLine.INSTANCE, "newline");
            }

            return code;
        }

        protected IStatement ParseStatement()
        {
            if (!ReachedEOF(1) && Peek(1).Type().Equals("operator") && ((LTOperator)Peek(1)).Value == Operator.Assign)
            {
                var name = Consume<LTLiteral>().Value;
                Require(Operator.Assign.ToToken(), "':='");
                var value = ParseExpression();

                return new StmtAssign(name, value);
            }
            else if (Now([(null, Keyword.VARIABLE.ToToken())], true))
            {
                var name = Consume<LTLiteral>().Value;
                Require(Operator.Set.ToToken(), "'='");
                var value = ParseExpression();

                return new StmtAssign(name, value);
            }
            else if (Now([(null, Keyword.CONST.ToToken())], true))
            {
                var name = Consume<LTLiteral>().Value;
                Require(Operator.Set.ToToken(), "'='");
                var value = ParseSingularExpression();

                return new StmtConst(name, value!);
            }
            else if (Now([(null, Keyword.DEFER.ToToken())], true))
            {
                var expr = ParseExpression();
                if (expr.Parts == null || !(expr.Parts[expr.Parts.Count - 1] is EPCall))
                    throw new ParserError("Expected a function call after 'defer'");

                return new StmtDefer(expr);
            }
            else if (Now([(null, Keyword.GO.ToToken())], true))
            {
                var expr = ParseExpression();
                if (expr.Parts == null || !(expr.Parts[expr.Parts.Count - 1] is EPCall))
                    throw new ParserError("Expected a function call after 'go'");

                return new StmtGo(expr);
            }
            else if (Now([(null, Keyword.RETURN.ToToken())], true))
            {
                if (Now([("newline", null)], false))
                {
                    return new StmtReturn(null);
                }
                else
                {
                    var values = new List<Expression>();

                    do
                    {
                        var expr = ParseExpression();
                        values.Add(expr);
                    } while (Now([(null, Operator.Comma.ToToken())], true));

                    return new StmtReturn(values);
                }
            }
            else
            {
                var expr = ParseExpression();

                if (Now([(null, Operator.Set.ToToken())], true))
                    if (expr.Parts != null && expr.Parts[expr.Parts.Count - 1] is EPCall)
                        throw new ParserError("Expected a reference before '='");
                    else return new StmtSet(expr, ParseExpression());
                else 
                    return new StmtExpression(expr);
            }
        }

        protected Expression ParseExpression()
        {
            IExpressionTarget? target;
            if ((target = ParseSingularExpression(optional: true)) != null)
                return new Expression(target, null, 0, Singular: true);
            else return ParsePartExpression();
        }

        protected IExpressionTarget? ParseSingularExpression(bool optional = false)
        {
            if (Now([("string", null)], false))
                return new ESTString(Consume<LTString>().Value);
            else if (Now([("integer", null)], false))
                return new ESTInteger(Consume<LTInteger>().Value);
            else if (Now([("float", null)], false))
                return new ESTFloat(Consume<LTFloat>().Value);
            else if (Now([(null, Keyword.TRUE.ToToken())], true))
                return ESTBoolean.TRUE;
            else if (Now([(null, Keyword.FALSE.ToToken())], true))
                return ESTBoolean.FALSE;
            else if (Now([(null, Keyword.NIL.ToToken())], true))
                return ESTNil.INSTANCE;
            else if (optional)
                return null;
            else throw new ParserError("Expected a singular expression");
        }

        protected int CountPointers()
        {
            var pointers = 0;

            while (true)
                if (Now([(null, Operator.Ampersand.ToToken())], true))
                    pointers++;
                else if (Now([(null, Operator.Star.ToToken())], true))
                    pointers--;
                else break;

            return pointers;
        }

        /**
         * I also like to call it the `friendly expression`, as it's friendly to [expression] parts
         */
        protected Expression ParsePartExpression()
        {
            var pointers = CountPointers();
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
                    continue;
                } else
                {
                    break;
                }
            }

            return new Expression(
                target,
                parts,
                pointers
            );
        }

        protected IExpressionTarget ParseExpressionTarget()
        {
            if (Now([("literal", null)], false))
            {
                var literal = Consume<LTLiteral>().Value;
                return new ETReference(literal);
            }
            else if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var expr = ParsePartExpression();
                Require(Operator.RParen.ToToken(), "')'");
                return new ETNest(expr);
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
