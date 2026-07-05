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

        protected static bool IsConvertibleType(Keyword kw)
        {
            return (kw == Keyword.RUNE ||
                    kw == Keyword.BYTE ||
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
    }
}
