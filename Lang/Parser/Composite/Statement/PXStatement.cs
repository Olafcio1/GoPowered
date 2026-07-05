using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;
using GoPowered.Lang.Parser.Token.ExprMath;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool Assigning();
        private partial bool Setting(int i = 0);

        protected partial IStatement ParseStatement()
        {
            if (Assigning())
                return ParseAssignment();
            else if (Now([(null, Keyword.VARIABLE.ToToken())], true))
                return ParseVar();
            else if (Now([(null, Keyword.CONST.ToToken())], true))
                return ParseConst();
            else if (Now([(null, Keyword.DEFER.ToToken())], true))
                return ParseDefer();
            else if (Now([(null, Keyword.GO.ToToken())], true))
                return ParseGo();
            else if (Now([(null, Keyword.RETURN.ToToken())], true))
                return ParseReturn();
            else if (Now([(null, Keyword.FOR.ToToken())], true))
                return ParseFor();
            else if (Now([(null, Keyword.IF.ToToken())], true))
                return ParseIf();
            else if (Now([(null, Keyword.CLOSE.ToToken())], true))
                return ParseClose();
            else if (Now([(null, Keyword.SELECT.ToToken())], true))
                return ParseSelect();
            else if (Now([(null, Keyword.SWITCH.ToToken())], true))
            {
                if (Now([(null, Operator.LCurly.ToToken())], false))
                    return ParseSwitch();
                else
                    return ParseValueSwitch();
            }
            else if (Now([(null, Keyword.FALLTHROUGH.ToToken())], true))
                return ParseFallthrough();
            else if (Now([(null, Keyword.CONTINUE.ToToken())], true))
                return ParseContinue();
            else if (Now([(null, Keyword.BREAK.ToToken())], true))
                return ParseBreak();
            else
                return ParseExpressionStmt();
        }

        private IStatement ParseExpressionStmt()
        {
            var anyexpr = ParseExpression();

            if (anyexpr is Expression expr)
            {
                //Console.WriteLine(expr);
                //Console.WriteLine(expr.Parts.Count);
                if (Now([(null, Operator.Set.ToToken())], true))
                {
                    if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                        throw new ParserError("Expected a reference before '='");
                    else return new StmtSet(expr, ParseExpression());
                }
                else if (Setting(-1))
                {
                    if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                        throw new ParserError("Expected a reference before '='");

                    List<Expression> extractTo = [expr];

                    while (Now([(null, Operator.Comma.ToToken())], true))
                    {
                        var another = ParseExpression();

                        if (another is not Expression anotherExpr)
                            throw new ParserError("Expected a reference");

                        if (anotherExpr.Parts != null && anotherExpr.Parts.Count > 0 && anotherExpr.Parts[^1] is EPCall)
                            throw new ParserError("Expected a reference");

                        extractTo.Add(anotherExpr);
                    }

                    Require(Operator.Set.ToToken(), "'='");

                    List<IAnyExpression> extractFrom = [];

                    do
                    {
                        extractFrom.Add(ParseExpression());
                    } while (Now([(null, Operator.Comma.ToToken())], true));

                    return new StmtExtractSet(extractFrom, extractTo);
                }
                else if (Now([(null, Operator.Increment.ToToken())], true))
                {
                    if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                        throw new ParserError("Expected a reference before '++'");
                    else return new StmtSet(expr, new MathExpression(expr, [new MathMember(MathMember.Arithmetic.Add, new Expression(new ESTInteger(1), null, 0, Singular: true))]));
                }
                else if (Now([(null, Operator.Decrement.ToToken())], true))
                {
                    if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                        throw new ParserError("Expected a reference before '--'");
                    else return new StmtSet(expr, new MathExpression(expr, [new MathMember(MathMember.Arithmetic.Subtract, new Expression(new ESTInteger(1), null, 0, Singular: true))]));
                }
                else if (Now([(null, Operator.Transmit.ToToken())], true))
                {
                    return new StmtChannelSend(expr, ParseExpression());
                }
                else
                {
                    var operations = new Dictionary<Operator, MathMember.IOperation>
                    {
                        // Arithmetic
                        [Operator.AddSet] = MathMember.Arithmetic.Add,
                        [Operator.SubtractSet] = MathMember.Arithmetic.Subtract,
                        [Operator.MultiplySet] = MathMember.Arithmetic.Multiply,
                        [Operator.DivideSet] = MathMember.Arithmetic.Divide,
                        [Operator.ModulusSet] = MathMember.Arithmetic.Modulus,
                        // Bitwise
                        [Operator.BAndSet] = MathMember.Bitwise.And,
                        [Operator.BOrSet] = MathMember.Bitwise.Or,
                        [Operator.BXorSet] = MathMember.Bitwise.Xor,
                        [Operator.BShiftLeftSet] = MathMember.Bitwise.ShiftLeft,
                        [Operator.BShiftRightSet] = MathMember.Bitwise.ShiftRight
                    };

                    foreach (var (@operator, operation) in operations)
                    {
                        if (Now([(null, @operator.ToToken())], true))
                        {
                            if (expr.Parts != null && expr.Parts.Count > 0 && expr.Parts[^1] is EPCall)
                                throw new ParserError($"Expected a reference before '{@operator.ToCode()}'");
                            else return new StmtSet(expr, new MathExpression(expr, [new MathMember(operation, ParseExpression(allowInit: false))]));
                        }
                    }
                }

                if (expr.Singular || expr.Parts!.Count == 0)
                    throw new ParserError("Expression statement with no parts");
                else if (expr.Parts[^1] is EPSquare || expr.Parts[^1] is EPMember)
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

        private partial StmtIf ParseIf();
        private partial IStatement ParseFor();
        private partial StmtReturn ParseReturn();
        private partial StmtGo ParseGo();
        private partial StmtDefer ParseDefer();
        private partial StmtConst ParseConst();
        private partial IStatement ParseVar();
        private partial IStatement ParseAssignment();
        private partial StmtClose ParseClose();
        private partial StmtSelect ParseSelect();
        private partial StmtSwitch ParseSwitch();
        private partial StmtSwitchValue ParseValueSwitch();
        private partial StmtFallthrough ParseFallthrough();
        private partial StmtContinue ParseContinue();
        private partial StmtBreak ParseBreak();
    }
}
