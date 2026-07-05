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
        private partial bool ParseNew(List<IExpressionPart> parts);

        protected bool ParseSquare(List<IExpressionPart> parts, bool allowInit)
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

                var ind = this.index;

                IAnyExpression? expr;

                try                 { expr = ParseExpression(); }
                catch (ParserError) { expr = null;              }

                int? ind1 = this.index;

                if (!Now([(null, Operator.RSquare.ToToken())], false))
                {
                    if (Now([(null, Operator.Colon.ToToken())], true))
                    {
                        var from = expr;
                        var to = Now([(null, Operator.RSquare.ToToken())], false)
                                    ? null
                                    : ParseExpression();

                        parts.Add(new EPSlice(from, to));
                        Require(Operator.RSquare.ToToken(), "']'");

                        return true;
                    }

                    expr = null;
                    ind1 = null;
                }

                this.index = ind;

                IType? type;

                try                 { type = ParseType(true); }
                catch (ParserError) { type = null;            }

                if (!Now([(null, Operator.RSquare.ToToken())], false))
                    type = null;

                if (expr == null && type == null)
                {
                    this.index = ind;
                    throw new ParserError("Expected expression (map/list access) or type (secondary parameters)");
                }

                if (ind1 != null && ind1 > this.index)
                    this.index = (int) ind1;

                parts.Add(new EPSquare(expr, type));
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

            #pragma warning disable CS0642

            while (true)
            {
                if (ParseCast(parts));
                else if (ParseMember(parts));
                else if (!constant && ParseSquare(parts, allowInit));
                else if (!constant && ParseCall(parts));
                else if (allowInit && !constant && ParseNew(parts));
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
