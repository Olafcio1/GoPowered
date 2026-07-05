using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETReference? ParseReference();
        protected partial ETConvert? ParseConvert(bool allowInit, bool constant);
        protected partial ETNest? ParseNest(bool allowInit, bool constant);
        protected partial ETMake? ParseMake(bool allowInit, bool constant);
        protected partial ETClosure? ParseClosure();
        protected partial ETImplicitStruct? ParseImplicitStruct();
        protected partial ETMap? ParseMap();
        protected partial ETSlice? ParseSlice();

        protected partial IExpressionTarget ParseExpressionTarget(bool allowInit = true, bool constant = false)
        {
            IExpressionTarget? target;

            #pragma warning disable CS0642
            if ((target = ParseReference()) != null);
            else if ((target = ParseConvert(allowInit, constant)) != null);
            else if ((target = ParseNest(allowInit, constant)) != null);
            else if ((target = ParseMake(allowInit, constant)) != null);
            else if ((target = ParseClosure()) != null);
            else if ((target = ParseImplicitStruct()) != null);
            else if ((target = ParseMap()) != null);
            else if ((target = ParseSlice()) != null);
            else
                // Should this error message be 'Expected an expression' instead?
                throw new ParserError("Expected expression target");

            return target;
            #pragma warning restore CS0642
        }
    }
}
