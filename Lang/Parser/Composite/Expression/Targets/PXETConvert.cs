using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Expr.Target;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial ETConvert? ParseConvert(bool allowInit, bool constant)
        {
            if (Now([("keyword", null)], false) && IsConvertibleType(((LTKeyword)Peek(0)).Value))
            {
                var name = Consume<LTKeyword>().Value.ToString().ToLower();
                //Console.WriteLine(name);

                Require(Operator.LParen.ToToken(), "'('");
                var value = ParseExpression(allowInit: allowInit, constant: constant);
                Require(Operator.RParen.ToToken(), "')'");

                return new ETConvert(name, value);
            }

            return null;
        }
    }
}
