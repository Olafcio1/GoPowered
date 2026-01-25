using GoPowered.Lang.Lexer.Char;
using GoPowered.Lang.Lexer.Token;

namespace GoPowered.Lang.Lexer
{
    public class Lexer
    {
        protected readonly string input;
        protected int index;
        public readonly List<ILexerToken> output;

        public Lexer(string input) {
            this.input = input;
            this.index = 0;
            this.output = [];
        }

        public List<ILexerToken> Lex()
        {
            while (!ReachedEOF())
            {
                #pragma warning disable CS0642
                if (LexNumber());
                else if (LexOperator());
                else if (LexLiteral());
                else if (LexString());
                else if (Now('\r') || Now('\n'))
                    // Newlines
                    output.Add(LTNewLine.INSTANCE);
                else if (Peek() <= 32)
                    // Control keys
                    index++;
                else if (Now("//"))
                    // Comments
                    while (Consume() != '\n');
                else if (Now("/*"))
                    // Multiline comments
                    while (!Now("*/")) ;
                else throw new LexerError("unexpected '" + Consume() + "'");
                #pragma warning restore CS0642
            }

            return output;
        }

        protected bool LexOperator()
        {
            foreach (var op in Operator.Values())
            {
                var str = op.ToCode();

                var ok = true;
                var i = 0;

                foreach (var ch in str)
                {
                    if (Peek(i++) != ch)
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    index += i;

                    output.Add(new LTOperator(op));
                    return true;
                }
            }

            return false;
        }

        protected bool LexLiteral()
        {
            var ch = Peek();
            if (!CharUtils.IsLatin(ch))
                return false;

            index++;

            var name = "" + ch;
            while (!ReachedEOF() && CharUtils.IsLiteral(Peek()))
                name += Consume();

            ILexerToken token;
            if (KeywordExtension.FromCode(name, out var value))
                token = value.ToToken();
            else token = new LTLiteral(name);

            output.Add(token);
            return true;
        }

        protected static readonly Dictionary<char, string> special = [];
        static Lexer() {
            special.Add('n', "\n");
            special.Add('r', "\r");
            special.Add('t', "\t");
            special.Add('b', "\b");
            special.Add('e', "\e");
            special.Add('a', "\a");
            special.Add('f', "\f");
        }

        protected bool LexString()
        {
            if (!Now('"'))
                return false;

            var value = "";
            while (true)
            {
                if (Now('"'))
                {
                    output.Add(new LTString(value));
                    return true;
                } else if (Now('\\'))
                {
                    var nxt = Consume();
                    if (nxt == 'u')
                        value += char.ConvertFromUtf32(int.Parse(Consume(4)));
                    else if (nxt == 'x')
                        value += (char)int.Parse(Consume(2));
                    else if (nxt == '"')
                        value += nxt;
                    else if (special.ContainsKey(nxt))
                        value += special.GetValueOrDefault(nxt);
                    else throw new LexerError("unrecognized \\" + nxt);
                } else
                {
                    value += Consume();
                }
            }

            throw new LexerError("unterminated string");
        }

        protected bool LexNumber()
        {
            var neg = Peek() == '-';

            var negindex = neg ? 1 : 0;
            var negmul = neg ? -1 : 1;

            if (Now("0x", negindex))
            {
                var value = GetText(CharUtils.IsHexDigit);
                if (value == "")
                    throw new LexerError("Empty hex number");

                output.Add(new LTInteger(Convert.ToInt64(value, 16) * negmul));
                return true;
            }
            else if (CharUtils.IsDigit(Peek(negindex)))
            {
                index += negindex;

                var value = GetText(CharUtils.IsDigit);
                if (GetFloatingPoint(out string? point, negindex))
                {
                    output.Add(new LTFloat(double.Parse(value + point) * negmul));
                    return true;
                } else
                {
                    output.Add(new LTInteger(long.Parse(value) * negmul));
                    return true;
                }
            }
            else if (GetFloatingPoint(out string? point, negindex))
            {
                output.Add(new LTFloat(double.Parse(point) * negmul));
                return true;
            }

            return false;
        }

        protected bool GetFloatingPoint(out string? point, int index = 0)
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

        protected string GetText(CharChecker checker)
        {
            var value = "";
            while (!ReachedEOF())
            {
                var ch = Peek();
                if (checker.Invoke(ch))
                {
                    value += ch;
                    index++;
                } else break;
            }

            return value;
        }

        protected char Consume()
        {
            return input[index++];
        }

        protected string Consume(int length)
        {
            return input.Substring(index, index += length);
        }

        protected char Peek(int after = 0)
        {
            return input[index + after];
        }

        protected string PeekString(int length, int after = 0)
        {
            var ind = index + after;

            length = Math.Min(length, input.Length - (ind + length));
            length = Math.Max(0, length);

            return input.Substring(ind, length);
        }

        protected bool Now(char ch, int after = 0)
        {
            if (Peek(after) == ch) {
                index += 1 + after;
                return true;
            } else {
                return false;
            }
        }

        protected bool Now(string value, int after = 0)
        {
            var len = value.Length;

            if (PeekString(len, after) == value)
            {
                index += len + after;
                return true;
            } else
            {
                return false;
            }
        }

        protected bool ReachedEOF() {
            return index >= input.Length;
        }
    }
}
