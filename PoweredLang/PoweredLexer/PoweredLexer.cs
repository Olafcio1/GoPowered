using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;
using GoPowered.PoweredLang.PoweredLexer.Token;

namespace GoPowered.PoweredLang.PoweredLexer
{
    public class PoweredLexer(string input)
               : Lexer(input)
    {
        public override bool LexLiteral()
        {
            var ch = Peek();
            if (!CharUtils.IsLatin(ch) && ch != '_')
                return false;

            Skip();

            var name = "" + ch;
            while (!ReachedEOF() && CharUtils.IsLiteral(Peek()))
                name += Consume();

            ILexerToken token;
            if (VKeywordExtension.FromCode(name, out var value1))
                token = value1.ToToken();
            else if (KeywordExtension.FromCode(name, out var value))
                token = value.ToToken();
            else token = new LTLiteral(name);

            AddToken(token);
            return true;
        }

        public override bool LexMulString()
        {
            if (!Now('`'))
                return false;

            var value = "";
            while (true)
            {
                if (Now('`'))
                {
                    if (Now('`'))
                    {
                        value += "`";
                    }
                    else
                    {
                        AddToken(new LTString(value));
                        return true;
                    }
                }
                else if (Now("${"))
                {
                    AddToken(new LTString(value));
                    value = "";

                    AddToken(new LTOperator(Operator.Plus));
                    AddToken(new LTOperator(Operator.LParen));

                    int open = 0;

                    while (true)
                    {
                        if (Peek() == '{')
                            open++;
                        else if (Peek() == '}')
                            if (open-- == 0)
                                break;

                        Lex1();
                    }

                    if (open == -1)
                        Skip();
                    else throw new LexerError("unterminated string interpolation");

                    AddToken(new LTOperator(Operator.RParen));
                    AddToken(new LTOperator(Operator.Plus));
                }
                else
                {
                    value += Consume();
                }
            }

            throw new LexerError("unterminated multiline string");
        }
    }
}
