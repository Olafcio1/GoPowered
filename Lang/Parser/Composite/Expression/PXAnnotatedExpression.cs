using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.AnnotatedExpr;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected virtual AnnotatedExpression ParseAnnotatedExpression()
        {
            var value = ParseExpression();
            string? annotation = null;

            if (Now([("string", null)]))
            {
                annotation = Consume<LTString>().Value;
            }

            return new AnnotatedExpression(value, annotation);
        }
    }
}
