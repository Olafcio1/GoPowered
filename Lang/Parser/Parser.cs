using GoPowered.Base;
using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Expr.Target;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.ExprMath;
using GoPowered.Lang.Parser.Token.Object;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;
using GoPowered.Lang.Parser.Token.Statement.Implementation.If;
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

        protected PTFunction ParseFunctionDef()
        {
            Require(Keyword.FUNCTION.ToToken(), "'func'");

            MethodParent? parent = null;

            if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var assign = (Peek(0) is LTLiteral && (Peek(1) is not LTOperator op || op.Value != Operator.RParen))
                                ? Consume<LTLiteral>().Value
                                : null;

                var type = ParseType();

                if (type is PointerType pointer) {
                    if (pointer.Type is not UniqueType)
                        throw new ParserError("A method's parent must be a struct or a pointer to a struct");
                } else if (type is not UniqueType) {
                    throw new ParserError("A method's parent must be a struct or a pointer to a struct");
                }

                parent = new MethodParent(assign, type);

                Require(Operator.RParen.ToToken(), "')'");
            }

            ParseFunctionSignature(out var name, out var args, out var returns, out var generics);

            var body = ParseCode();
            return new PTFunction(name, args, body, returns, parent, generics);
        }

        protected void ParseFunctionSignature(out string name, out List<Argument> args, out List<ReturnValue>? returns, out Dictionary<string, IType>? generics)
        {
            name = Consume<LTLiteral>().Value;
            args = new List<Argument>();

            generics = ParseDefGenerics();

            Require(Operator.LParen.ToToken(), "'('");

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

            if (Now([(null, Operator.Set.ToToken())], true))
                return new PTTypeAlias(name, ParseType()!);

            Dictionary<string, IType>? generics = ParseDefGenerics();

            if (Now([(null, Keyword.STRUCT.ToToken())], true))
                return ParseTypeStruct(name, generics);
            else if (Now([(null, Keyword.INTERFACE.ToToken())], true))
                return ParseTypeInterface(name, generics);
            else if (ParseType_out(out IType? type, optional: true))
                return new PTTypeClone(name, type!, generics);
            else if (Now([(null, Operator.Set.ToToken())], true))
                throw new ParserError("A type alias cannot use generics");
            else throw new ParserError("Expected a struct, interface, type alias or type clone");
        }

        private Dictionary<string, IType>? ParseDefGenerics()
        {
            Dictionary<string, IType>? generics = null;

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

                    var aName = Consume<LTLiteral>().Value;
                    var aParent = ParseType()!;

                    generics[aName] = aParent;
                }
            }

            return generics;
        }

        protected PTTypeStruct ParseTypeStruct(string name, Dictionary<string, IType>? generics)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            var Struct = new PTTypeStruct(name, [], [], generics);

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;

                if (Now([("literal", null), ("newline", null)], false))
                {
                    // Struct Inherit
                    Struct.Inherits.Add(Consume<LTLiteral>().Value);
                    continue;
                }

                var fNames = new List<string>();

                do
                {
                    fNames.Add(Consume<LTLiteral>().Value);
                } while (Now([(null, Operator.Comma.ToToken())], true));

                var fType = ParseType();

                foreach (var fName in fNames)
                    Struct.Fields[fName] = fType!;
            }

            return Struct;
        }

        protected PTTypeInterface ParseTypeInterface(string name, Dictionary<string, IType>? generics)
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

                ParseFunctionSignature(out var fName, out var fArgs, out var fReturns, out var fGenerics);
                Interface.Methods.Add(new FunctionSignature(fName, fArgs, fReturns, fGenerics));
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
            else if (Now([(null, Keyword.CHAR.ToToken())], true))
            {
                return PrimitiveType.CHAR;
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

        private bool Assigning()
        {
            var i = 0;

            while (!ReachedEOF(i + 2))
            {
                if (Peek(i++) is not LTLiteral)
                    break;

                if (Peek(i++) is not LTOperator op)
                    break;

                if (op.Value == Operator.Comma)
                    continue;
                else if (op.Value == Operator.Assign)
                    return true;
                else
                    break;
            }

            return false;
        }

        protected IStatement ParseStatement()
        {
            if (Assigning())
            {
                var names = new List<string>();

                do
                {
                    names.Add(Consume<LTLiteral>().Value);
                } while (Now([(null, Operator.Comma.ToToken())], true));

                Require(Operator.Assign.ToToken(), "':='");

                var value = ParseExpression();

                return names.Count == 1
                        ? new StmtAssign(names[0], value, null)
                        : new StmtExtractAssign(names, value, null);
            }
            else if (Now([(null, Keyword.VARIABLE.ToToken())], true))
            {
                if (Now([(null, Operator.LParen.ToToken())], true))
                {
                    var assignment = new StmtMultiAssign([]);

                    while (true)
                    {
                        ConsumeNewlines();

                        if (Now([(null, Operator.RParen.ToToken())], true))
                            break;

                        ParseVarDefinition(out var name, out var value, out var type);

                        assignment.Variables.Add(new Assignment(name, value, type));
                    }

                    return assignment;
                }
                else
                {
                    ParseVarDefinition(out var name, out var value, out var type);

                    return new StmtAssign(name, value, type);
                }
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
                var expr = ParseObjectExpression();
                if (expr.Parts == null || expr.Parts[^1] is not EPCall)
                    throw new ParserError("Expected a function call after 'defer'");

                return new StmtDefer(expr);
            }
            else if (Now([(null, Keyword.GO.ToToken())], true))
            {
                var expr = ParseObjectExpression();
                if (expr.Parts == null || expr.Parts[^1] is not EPCall)
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
                    var values = new List<IAnyExpression>();

                    do
                    {
                        var expr = ParseExpression();
                        values.Add(expr);
                    } while (Now([(null, Operator.Comma.ToToken())], true));

                    return new StmtReturn(values);
                }
            }
            else if (Now([(null, Keyword.FOR.ToToken())], true))
            {
                var i = 0;
                var collectVariables = false;

                while (true)
                {
                    if (
                            Peek(i) is LTLiteral &&
                            Peek(i + 1) is LTOperator op && op.Value == Operator.Comma
                    )
                    {
                        i += 2;
                        continue;
                    } else if (
                            Peek(i) is LTOperator op2 &&
                            op2.Value == Operator.Assign
                    )
                    {
                        i += 1;
                        collectVariables = true;
                    } else if (
                            Peek(i++) is LTOperator op3 &&
                            op3.Value == Operator.RCurly
                    )
                    {
                        break;
                    }
                }

                List<string?>? variables = null;

                if (collectVariables)
                {
                    variables = [];

                    var comma = false;

                    while (true)
                    {
                        if (Now([(null, Operator.Assign.ToToken())], true))
                            break;
                        else if (!comma)
                            comma = true;
                        else Require(Operator.Comma.ToToken(), "','");

                        var name = Consume<LTLiteral>().Value;
                        if (name == "_")
                            name = null;

                        variables.Add(name);
                    }
                }

                Require(Keyword.RANGE.ToToken(), "'range'");

                var rangeThrough = ParseObjectExpression(false);
                var effect = ParseCode();

                return new StmtForRange(variables, rangeThrough, effect);
            }
            else if (Now([(null, Keyword.IF.ToToken())], true))
            {
                var branches = new List<Branch>();
                var first = true;

                while (true)
                {
                    if (first)
                        first = false;
                    else
                    {
                        var i = 0;
                        var ok = false;

                        while (true)
                        {
                            if (Peek(i++) is LTNewLine)
                                continue;

                            if (!(Peek(i - 1) is LTKeyword kw1 && kw1.Value == Keyword.ELSE))
                                goto postBranches;

                            if (!(Peek(i) is LTKeyword kw2 && kw2.Value == Keyword.IF))
                                goto postBranches;

                            i++;
                            ok = true;

                            break;
                        }

                        if (!ok)
                            goto postBranches;

                        this.index += i;
                    }

                    var branch = new Branch();

                    var expr = ParseExpression(allowInit: false);
                    var cond = expr is Condition cast
                                ? cast
                                : new Condition(expr, ConditionType.EQUAL, new Expression(ESTBoolean.TRUE, null, 0, Singular: true));

                    var effect = ParseCode();

                    branch.Cond = cond;
                    branch.Effect = effect;

                    branches.Add(branch);
                }

                postBranches:

                var otherwise = Now([(null, Keyword.ELSE.ToToken())], true)
                                  ? ParseCode()
                                  : null;

                return new StmtIf(branches, otherwise);
            }
            else
            {
                var anyexpr = ParseExpression();

                if (anyexpr is Expression expr)
                {
                    //Console.WriteLine(expr);
                    //Console.WriteLine(expr.Parts.Count);
                    if (Now([(null, Operator.Set.ToToken())], true))
                        if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                            throw new ParserError("Expected a reference before '='");
                        else return new StmtSet(expr, ParseExpression());
                    else if (expr.Singular || expr.Parts!.Count == 0)
                        throw new ParserError("Expression statement with no parts");
                    else if (expr.Parts[^1] is EPAccess || expr.Parts[^1] is EPMember)
                        throw new ParserError("Expression statement ends with unnecessary element/member access");
                    else if (expr.Parts[^1] is EPSlice)
                        throw new ParserError("Expression statement ends with unnecessary slice");

                    return new StmtExpression(expr);
                }
                else
                {
                    throw new ParserError("A math expression cannot be used as a statement");
                }
            }
        }

        private void ParseVarDefinition(out string name, out IAnyExpression? value, out IType? type)
        {
            name = Consume<LTLiteral>().Value;

            value = null;
            type = null;

            if (!Now([(null, Operator.Set.ToToken())], false))
                type = ParseType();

            if (Now([(null, Operator.Set.ToToken())], true))
                value = ParseExpression();
        }

        protected Expression ParseObjectExpression(bool allowInit = true)
        {
            return (Expression) ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit);
        }

        protected IAnyExpression ParseExpression(bool allowMath = true, bool allowLogic = true, bool allowInit = true)
        {
            IExpressionTarget? target;
            IAnyExpression expr;

            bool logicNeg = Now([(null, Operator.LNot.ToToken())], true);
            bool mathNeg = Now([(null, Operator.Minus.ToToken())], true);

            if (logicNeg && mathNeg)
                throw new ParserError("A logical and math negation cannot be used together");

            if ((target = ParseSingularExpression(optional: true)) != null)
                expr = new Expression(target, null, 0, Singular: true);
            else expr = ParsePartExpression(allowInit: allowInit);

                 if (logicNeg) expr = new LNegate(expr);
            else if (mathNeg)  expr = new MNegate(expr);

            if (
                    allowMath &&
                    Peek(0) is LTOperator op &&
                    (
                            op.Value == Operator.Plus ||
                            op.Value == Operator.Minus ||
                            op.Value == Operator.Star ||
                            op.Value == Operator.Slash ||
                            op.Value == Operator.Modulus
                            //op.Value == Operator.Exponentiate
                    )
            )
            {
                var math = new MathExpression(expr, []);

                while (true)
                {
                    if (Now([(null, Operator.Plus.ToToken())], true))
                    {
                        math.Members.Add(new MathMember(MathMember.TypeEnum.Add, (Expression) ParseExpression(allowMath: false)));
                    }
                    else if (Now([(null, Operator.Minus.ToToken())], true))
                    {
                        math.Members.Add(new MathMember(MathMember.TypeEnum.Subtract, (Expression) ParseExpression(allowMath: false)));
                    }
                    else if (Now([(null, Operator.Star.ToToken())], true))
                    {
                        math.Members.Add(new MathMember(MathMember.TypeEnum.Multiply, (Expression) ParseExpression(allowMath: false)));
                    }
                    else if (Now([(null, Operator.Slash.ToToken())], true))
                    {
                        math.Members.Add(new MathMember(MathMember.TypeEnum.Divide, (Expression) ParseExpression(allowMath: false)));
                    }
                    else if (Now([(null, Operator.Modulus.ToToken())], true))
                    {
                        math.Members.Add(new MathMember(MathMember.TypeEnum.Modulus, (Expression) ParseExpression(allowMath: false)));
                    }
                    else
                    {
                        break;
                    }
                }

                expr = math;
            }

            if (allowLogic)
            {
                if (Now([(null, Operator.EqualTo.ToToken())], true))
                    return new Condition(expr, ConditionType.EQUAL, ParseExpression(allowInit: allowInit));
                else if (Now([(null, Operator.NotEqual.ToToken())], true))
                    return new Condition(expr, ConditionType.NOT_EQUAL, ParseExpression(allowInit: allowInit));
                else if (Now([(null, Operator.GreaterThan.ToToken())], true))
                    return new Condition(expr, ConditionType.GREATER_THAN, ParseExpression(allowInit: allowInit));
                else if (Now([(null, Operator.GreaterOrEqual.ToToken())], true))
                    return new Condition(expr, ConditionType.GREATER_OR_EQUAL, ParseExpression(allowInit: allowInit));
                else if (Now([(null, Operator.LessThan.ToToken())], true))
                    return new Condition(expr, ConditionType.LESS_THAN, ParseExpression(allowInit: allowInit));
                else if (Now([(null, Operator.LessOrEqual.ToToken())], true))
                    return new Condition(expr, ConditionType.LESS_OR_EQUAL, ParseExpression(allowInit: allowInit));
            }

            return expr;
        }

        protected IExpressionTarget? ParseSingularExpression(bool optional = false)
        {
            if (Now([("string", null)], false))
                return new ESTString(Consume<LTString>().Value);
            else if (Now([("char", null)], false))
                return new ESTChar(Consume<LTChar>().Value);
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
        protected Expression ParsePartExpression(bool allowInit = true)
        {
            var pointers = CountPointers();
            var target = ParseExpressionTarget();
            var parts = new List<IExpressionPart>();

            List<IType>? generics = null;

            while (true)
            {
                if (Now([(null, Operator.Dot.ToToken())], true))
                {
                    parts.Add(new EPMember(Consume<LTLiteral>().Value));
                }
                else if (Now([(null, Operator.LSquare.ToToken())], true))
                {
                    if (Now([(null, Operator.Colon.ToToken())], true))
                    {
                        // Slice without 'from'
                        var to = Now([(null, Operator.RSquare.ToToken())], false)
                                    ? null
                                    : ParseExpression();

                        parts.Add(new EPSlice(null, to));

                        Require(Operator.RSquare.ToToken(), "']'");
                        continue;
                    }

                    IAnyExpression expr;

                    var ind = this.index;

                    try
                    {
                        expr = ParseExpression();
                    } catch (ParserError)
                    {
                        if (!allowInit)
                            throw;

                        var failing = this.index;
                        this.index = ind;

                        var type = ParseType(true) ??
                                   throw new ParserError("Expected expression (map/list access) or type (secondary parameters)");

                        var orig = generics;
                        var point = this.index;
                        var repeated = (generics != null);

                        generics = [type];

                        while (true)
                        {
                            if (Now([(null, Operator.RSquare.ToToken())], true))
                                break;
                            else Require(Operator.Comma.ToToken(), "','");

                            generics.Add(ParseType()!);
                        }

                        if (!Now([(null, Operator.LCurly.ToToken())], false))
                        {
                            generics = orig;
                            this.index = failing;
                            throw;
                        }

                        if (repeated)
                        {
                            this.index = point;
                            throw new ParserError("Type parameters have already been provided");
                        }

                        continue;
                    }

                    if (Peek(1) is LTOperator op && op.Value == Operator.LCurly && allowInit)
                    {
                        this.index = ind;

                        if (generics != null)
                            throw new ParserError("Type parameters have already been provided");

                        generics = [];

                        var comma = false;

                        while (true)
                        {
                            if (Now([(null, Operator.RSquare.ToToken())], true))
                                break;
                            else if (comma)
                                Require(Operator.Comma.ToToken(), "','");
                            else comma = true;

                            generics.Add(ParseType()!);
                        }

                        continue;
                    }

                    if (Now([(null, Operator.Colon.ToToken())], true))
                    {
                        var from = expr;
                        var to = Now([(null, Operator.RSquare.ToToken())], false)
                                    ? null
                                    : ParseExpression();

                        parts.Add(new EPSlice(from, to));
                    }
                    else
                    {
                        parts.Add(new EPAccess(expr));
                    }

                    Require(Operator.RSquare.ToToken(), "']'");
                }
                else if (Now([(null, Operator.LParen.ToToken())], true))
                {
                    var args = new List<Parameter>();
                    var comma = false;

                    while (true)
                    {
                        if (Now([(null, Operator.RParen.ToToken())], true))
                            break;
                        else if (comma)
                            Require(Operator.Comma.ToToken(), "','");
                        else comma = true;

                        var value = ParseExpression();
                        var ellipsis = Now([(null, Operator.Ellipsis.ToToken())], true);

                        args.Add(new Parameter(value, ellipsis));
                    }

                    parts.Add(new EPCall(args));
                    continue;
                }
                else if (allowInit && Now([(null, Operator.LCurly.ToToken())], true))
                {
                    var positional = new List<IAnyExpression>();
                    var keyword = new Dictionary<string, IAnyExpression>();

                    var comma = false;
                    var newlines = false;

                    while (true)
                    {
                        ConsumeNewlines(ref newlines);

                        if (!newlines && Now([(null, Operator.RCurly.ToToken())], true))
                            break;
                        else if (comma)
                            Require(Operator.Comma.ToToken(), "','");
                        else comma = true;

                        ConsumeNewlines(ref newlines);

                        if (Now([(null, Operator.RCurly.ToToken())], true))
                            break;

                        ConsumeNewlines(ref newlines);

                        if (Now([("literal", null), (null, Operator.Colon.ToToken())], false))
                        {
                            var name = Consume<LTLiteral>().Value;
                            Require(Operator.Colon.ToToken(), "':'");
                            var value = ParseExpression();

                            keyword[name] = value;
                        } else
                        {
                            if (keyword.Count > 0)
                                throw new ParserError("Cannot provide positional parameters after keyword parameters");

                            positional.Add(ParseExpression());
                        }
                    }

                    parts.Add(new EPNew(positional, keyword, generics));
                    generics = null;

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
            else if (Now([("keyword", null)], false) && IsCastableType(((LTKeyword)Peek(0)).Value))
            {
                var name = Consume<LTKeyword>().Value.ToString().ToLower();
                //Console.WriteLine(name);

                Require(Operator.LParen.ToToken(), "'('");
                var value = ParseExpression();
                Require(Operator.RParen.ToToken(), "')'");

                return new ETCast(name, value);
            }
            else if (Now([(null, Operator.LParen.ToToken())], true))
            {
                var expr = ParsePartExpression();
                Require(Operator.RParen.ToToken(), "')'");
                return new ETNest(expr);
            }
            else if (Now([(null, Keyword.MAKE.ToToken())], true))
            {
                Require(Operator.LParen.ToToken(), "'('");

                var type = ParseType()!;
                var args = new List<IAnyExpression>();

                while (true)
                {
                    if (Now([(null, Operator.RParen.ToToken())], true))
                        break;

                    Require(Operator.Comma.ToToken(), "','");

                    args.Add(ParseExpression());
                }

                return new ETMake(type, args);
            }
            else
            {
                // Should this error message be 'Expected an expression' instead?
                throw new ParserError("Expected expression target");
            }
        }

        protected static bool IsCastableType(Keyword kw)
        {
            return (kw == Keyword.CHAR || 
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

                            output.Add(new PTImport(Consume<LTString>().Value));
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
