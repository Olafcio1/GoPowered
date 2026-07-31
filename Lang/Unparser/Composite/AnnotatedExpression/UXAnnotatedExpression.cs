using GoPowered.Lang.Parser.Token.AnnotatedExpr;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleAnnotatedExpression(AnnotatedExpression expr)
        {
            var value = "";

            value += HandleAnyExpression(expr.Expression);

            if (expr.Annotation != null)
            {
                value += " `";
                value += expr.Annotation.Replace("`", "\\`").Replace("\n", "\\n").Replace("\r", "\\r");
                value += "`";
            }

            return value;
        }
    }
}
