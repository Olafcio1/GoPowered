using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.ExprLogic;
using GoPowered.Lang.Parser.Token.ExprMath;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial IAnyExpression ParseExpression(bool allowMath = true, bool allowLogic = true, bool allowInit = true, bool constant = false)
        {
            IExpressionTarget? target;
            IAnyExpression expr;

            bool logicNeg = Now([(null, Operator.LNot.ToToken())], true);
            bool mathNeg = Now([(null, Operator.Minus.ToToken())], true);

            if (logicNeg && mathNeg)
                throw new ParserError("A logical and math negation cannot be used together");

            if ((target = ParseSingularExpression(optional: true)) != null)
                expr = new Expression(target, null, 0, Singular: true);
            else expr = ParsePartExpression(allowInit: allowInit, constant: constant);

            if (logicNeg) expr = new LNegate(expr);
            else if (mathNeg) expr = new MNegate(expr);

            if (
                    allowMath &&
                    Peek(0) is LTOperator op &&
                    (
                            op.Value == Operator.Plus ||
                            op.Value == Operator.Minus ||
                            op.Value == Operator.Star ||
                            op.Value == Operator.Slash ||
                            op.Value == Operator.Modulus ||
                            //op.Value == Operator.Exponentiate ||
                            op.Value == Operator.Ampersand ||
                            op.Value == Operator.VLine ||
                            op.Value == Operator.BXor ||
                            op.Value == Operator.BShiftLeft ||
                            op.Value == Operator.BShiftRight
                    )
            )
            {
                expr = ParseMathExpression(allowInit, constant, expr);
            }

            if (allowLogic)
            {
                ICondition? cond = ParseLogicExpression(allowInit, constant, expr);

                if (cond != null)
                    return cond;
            }

            return expr;
        }

        private ICondition? ParseLogicExpression(bool allowInit, bool constant, IAnyExpression expr)
        {
            ICondition? cond = null;

            if (Now([(null, Operator.EqualTo.ToToken())], true))
                cond = new Condition(expr, ConditionType.EQUAL, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));
            else if (Now([(null, Operator.NotEqual.ToToken())], true))
                cond = new Condition(expr, ConditionType.NOT_EQUAL, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));
            else if (Now([(null, Operator.GreaterThan.ToToken())], true))
                cond = new Condition(expr, ConditionType.GREATER_THAN, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));
            else if (Now([(null, Operator.GreaterOrEqual.ToToken())], true))
                cond = new Condition(expr, ConditionType.GREATER_OR_EQUAL, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));
            else if (Now([(null, Operator.LessThan.ToToken())], true))
                cond = new Condition(expr, ConditionType.LESS_THAN, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));
            else if (Now([(null, Operator.LessOrEqual.ToToken())], true))
                cond = new Condition(expr, ConditionType.LESS_OR_EQUAL, ParseExpression(allowLogic: false, allowInit: allowInit, constant: constant));

            if (Now([(null, Operator.LAnd.ToToken())], true))
            {
                cond ??= AsCondition(expr);
                cond = new LBoth(cond, ParseCondition(allowInit: allowInit));
            }

            if (Now([(null, Operator.LOr.ToToken())], true))
            {
                cond ??= AsCondition(expr);
                cond = new LEither(cond, ParseCondition(allowInit: allowInit));
            }

            return cond;
        }

        private IAnyExpression ParseMathExpression(bool allowInit, bool constant, IAnyExpression expr)
        {
            var math = new MathExpression(expr, []);

            while (true)
            {
                if (Now([(null, Operator.Plus.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Arithmetic.Add, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.Minus.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Arithmetic.Subtract, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.Star.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Arithmetic.Multiply, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.Slash.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Arithmetic.Divide, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.Modulus.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Arithmetic.Modulus, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.Ampersand.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Bitwise.And, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.VLine.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Bitwise.Or, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.BXor.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Bitwise.Xor, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.BShiftLeft.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Bitwise.ShiftLeft, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else if (Now([(null, Operator.BShiftRight.ToToken())], true))
                {
                    ConsumeNewlines();
                    math.Members.Add(new MathMember(MathMember.Bitwise.ShiftRight, (Expression)ParseExpression(allowMath: false, allowLogic: false, allowInit: allowInit, constant: constant)));
                }
                else
                {
                    break;
                }
            }

            expr = math;
            return expr;
        }
    }
}
