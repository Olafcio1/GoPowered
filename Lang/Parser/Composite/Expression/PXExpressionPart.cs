using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Type;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        private partial bool ParseCast(List<IExpressionPart> parts);
        private partial bool ParseMember(List<IExpressionPart> parts);
        private partial bool ParseCall(List<IExpressionPart> parts);
        private partial bool ParseNew(List<IExpressionPart> parts, ref List<IType>? generics);

        protected bool ParseSquare(List<IExpressionPart> parts, ref List<IType>? generics, bool allowInit)
        {
            if (Now([(null, Operator.LSquare.ToToken())], true))
            {
                if (Now([(null, Operator.Colon.ToToken())], true))
                {
                    // Slice without 'from'
                    var to = Now([(null, Operator.RSquare.ToToken())], false)
                                ? null
                                : ParseExpression();

                    parts.Add(new EPSlice(null, to));

                    Require(Operator.RSquare.ToToken(), "']'");
                    return true;
                }

                IAnyExpression expr;

                var ind = this.index;

                try
                {
                    expr = ParseExpression();
                }
                catch (ParserError)
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

                    return true;
                }

                if (Peek(0) is LTOperator op && (op.Value == Operator.Comma || op.Value == Operator.LCurly) && allowInit)
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

                    return true;
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

                return true;
            }

            return false;
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
        protected partial Expression ParsePartExpression(bool allowInit = true, bool constant = false)
        {
            var pointers = CountPointers();
            var target = ParseExpressionTarget(allowInit: allowInit, constant: constant);
            var parts = new List<IExpressionPart>();

            List<IType>? generics = null;

            #pragma warning disable CS0642

            while (true)
            {
                if (ParseCast(parts));
                else if (ParseMember(parts));
                else if (!constant && ParseSquare(parts, ref generics, allowInit));
                else if (!constant && ParseCall(parts));
                else if (allowInit && !constant && ParseNew(parts, ref generics));
                else
                {
                    break;
                }
            }

            #pragma warning restore CS0642

            return new Expression(
                target,
                parts,
                pointers
            );
        }
    }
}
