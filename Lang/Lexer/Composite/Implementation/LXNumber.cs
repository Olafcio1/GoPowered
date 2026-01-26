using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;
using System;

namespace GoPowered.Lang.Lexer.Composite.Implementation
{
    public sealed class LXNumber : CLexer
    {
        internal LXNumber(PeekT peek,
                           ConsumeCharT consumeChar,
                           ConsumeStringT consumeString,
                           SkipT skip,
                           AddTokenT addToken,
                           ReachedEOFT reachedEOF,
                           NowCharT nowChar,
                           NowStringT nowString) : base(peek, consumeChar, consumeString, skip, addToken, reachedEOF, nowChar, nowString)
        {}

        public bool LexNumber()
        {
            var vneg = Peek() == '-';
            var vpos = Peek() == '+';

            var negindex = (vneg || vpos) ? 1 : 0;
            var negmul = vneg ? -1 : 1;

            if (Now("0x", negindex))
            {
                var value = GetText(CharUtils.IsHexDigit);
                if (value == "")
                    throw new LexerError("Empty hex number");

                AddToken(new LTInteger(Convert.ToInt64(value, 16) * negmul));
                return true;
            }
            else if (CharUtils.IsDigit(Peek(negindex)))
            {
                Skip(negindex);

                var value = GetText(CharUtils.IsDigit);
                if (GetFloatingPoint(out string? point, negindex))
                {
                    AddToken(new LTFloat(double.Parse(value + point) * negmul));
                    return true;
                }
                else
                {
                    AddToken(new LTInteger(long.Parse(value) * negmul));
                    return true;
                }
            }
            else if (
                    !ReachedEOF(2) &&
                    CharUtils.IsDigit(Peek(negindex + 1)) &&
                    GetFloatingPoint(out string? point, negindex)
            )
            {
                AddToken(new LTFloat(double.Parse(point!) * negmul));
                return true;
            }

            return false;
        }

        private bool GetFloatingPoint(out string? point, int index = 0)
        {
            if (Now('.', index))
            {
                point = ".";

                var str = GetText(CharUtils.IsDigit);
                if (str == "")
                    throw new LexerError("Empty floating point");

                point += str;
                return true;
            }
            else
            {
                point = null;
                return false;
            }
        }

        private string GetText(CharChecker checker)
        {
            var value = "";
            while (!ReachedEOF())
            {
                var ch = Peek();
                if (checker.Invoke(ch))
                {
                    value += ch;
                    Skip();
                }
                else break;
            }

            return value;
        }
    }
}
