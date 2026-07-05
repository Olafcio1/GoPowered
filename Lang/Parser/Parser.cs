using GoPowered.Base;
using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.Object;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser : BaseParser<ILexerToken, ParserError>
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
                else if (Now([(null, Keyword.VARIABLE.ToToken())]) || Now([(null, Keyword.CONST.ToToken())]))
                    output.Add(ParseStatement());
                else
                    throw new ParserError("Expected a global statement");
            }
        }

        protected void ConsumeNewlines()
        {
            while (Now([("newline", null)]))
                Consume();
        }

        protected void ConsumeNewlines(ref bool used)
        {
            while (Now([("newline", null)]))
            {
                Consume();

                used = true;
            }
        }

        protected partial PTFunction ParseFunctionDef();

        protected partial void ParseFunctionSignature(out List<Argument> args, out List<ReturnValue>? returns, out Dictionary<string, IType>? generics);

        protected IParserToken ParseTypeDef()
        {
            Require(Keyword.TYPE.ToToken(), "'type'");

            var name = Consume<LTLiteral>().Value;

            if (Now([(null, Operator.Set.ToToken())], true))
                return new PTTypeAlias(name, ParseType()!);

            Dictionary<string, IType>? generics = ParseDefGenerics(hard: false);

            if (Now([(null, Keyword.STRUCT.ToToken())], true))
            {
                ParseTypeStruct(out var fields, out var inherits);
                return new PTTypeStruct(name, fields, inherits, generics);
            }
            else if (Now([(null, Keyword.INTERFACE.ToToken())], true))
            {
                ParseTypeInterface(out var methods, out var inherits);
                return new PTTypeInterface(name, methods, inherits, generics);
            }
            else if (ParseType_out(out IType? type, optional: true))
                return new PTTypeClone(name, type!, generics);
            else if (Now([(null, Operator.Set.ToToken())], true))
                throw new ParserError("A type alias cannot use generics");
            else throw new ParserError("Expected a struct, interface, type alias or type clone");
        }

        private Dictionary<string, IType>? ParseDefGenerics(bool hard = true)
        {
            Dictionary<string, IType>? generics = null;

            if (Now([(null, Operator.LSquare.ToToken())], false) && (hard || (!ReachedEOF(3) && (Peek(1) is not LTOperator op || op.Value != Operator.RSquare)
                                                                                             && (Peek(2) is not LTOperator op2 || op2.Value != Operator.RSquare))))
            {
                index++;
                generics = [];

                var comma = false;

                while (true)
                {
                    if (Now([(null, Operator.RSquare.ToToken())], true))
                        break;
                    else if (!comma)
                        comma = true;
                    else Require(Operator.Comma.ToToken(), "','");

                    var aName = Consume<LTLiteral>().Value;
                    var aParent = ParseType()!;

                    generics[aName] = aParent;
                }
            }

            return generics;
        }

        protected partial void ParseTypeStruct(out Dictionary<string, IType> Fields, out List<string> Inherits);

        protected partial void ParseTypeInterface(out Dictionary<string, FunctionSignature> Methods, out List<string> Inherits);

        protected bool ParseType_out(out IType? type, bool optional)
        {
            type = ParseType(optional);
            return type != null;
        }

        protected partial IType? ParseType(bool optional = false);

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
                Require(LTNewLine.INSTANCE, "newline");
            }

            return code;
        }

        protected partial IStatement ParseStatement();

        protected Expression ParseObjectExpression(bool allowInit = true)
        {
            return (Expression) ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit);
        }

        protected partial IAnyExpression ParseExpression(bool allowMath = true, bool allowLogic = true, bool allowInit = true, bool constant = false);

        protected Condition ParseCondition(bool allowInit = true)
        {
            var expr = ParseExpression(allowInit: allowInit);
            var cond = expr is Condition cast
                        ? cast
                        : AsCondition(expr);

            return cond;
        }

        private static Condition AsCondition(IAnyExpression expr)
        {
            return new Condition(expr, ConditionType.EQUAL, new Expression(ESTBoolean.TRUE, null, 0, Singular: true));
        }

        protected partial IExpressionTarget? ParseSingularExpression(bool optional = false);

        /**
         * I also like to call it the `friendly expression`, as it's friendly to [expression] parts
         */
        protected partial Expression ParsePartExpression(bool allowInit = true, bool constant = false);

        protected partial IExpressionTarget ParseExpressionTarget(bool allowInit = true, bool constant = false);

        protected static bool IsConvertibleType(Keyword kw)
        {
            return (kw == Keyword.RUNE ||
                    kw == Keyword.INT ||
                    kw == Keyword.INT64 ||
                    kw == Keyword.INT32 ||
                    kw == Keyword.INT16 ||
                    kw == Keyword.INT8 ||
                    kw == Keyword.UINT ||
                    kw == Keyword.UINT64 ||
                    kw == Keyword.UINT32 ||
                    kw == Keyword.UINT16 ||
                    kw == Keyword.UINT8 ||
                    kw == Keyword.FLOAT ||
                    kw == Keyword.FLOAT64 ||
                    kw == Keyword.FLOAT32);
        }

        protected void CollectImports()
        {
            while (!ReachedEOF())
            {
                if (Now([(null, Keyword.IMPORT.ToToken())], true))
                {
                    if (Now([(null, Operator.LParen.ToToken())], true))
                    {
                        bool empty = true;

                        while (true)
                        {
                            if (Now([(null, Operator.RParen.ToToken())], true))
                                break;
                            else if (Now([("newline", null)], true))
                                continue;

                            if (Now([("literal", null)], false))
                            {
                                var alias = Consume<LTLiteral>().Value;
                                var package = Consume<LTString>().Value;

                                if (alias == "_")
                                    alias = null;

                                output.Add(new PTImportAs(package, alias));
                            }
                            else
                            {
                                output.Add(new PTImport(Consume<LTString>().Value));
                            }

                            Require(LTNewLine.INSTANCE, "newline");

                            empty = false;
                        }

                        if (empty)
                            throw new ParserError("Empty import section");
                    }
                    else
                    {
                        output.Add(new PTImport(Consume<LTString>().Value));
                    }
                }
                else if (Now([("newline", null)], true))

                    continue;

                else break;
            }
        }
    }
}
