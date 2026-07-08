using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;
using GoPowered.Lang.Parser.Type;
using GoPowered.PoweredLang.PoweredLexer.Token;
using GoPowered.PoweredLang.PoweredParser.Token.Object;

namespace GoPowered.PoweredLang.PoweredParser
{
    public class PoweredParser(List<ILexerToken> input)
               : Parser(input)
    {
        protected override IParserToken ParseTypeDef_Post(string name, Dictionary<string, IType>? generics)
        {
            if (Now([(null, VKeyword.OBJECT.ToToken())], true))
            {
                ParseTypeStruct(out var fields, out var inherits);
                return new PTTypeObject(name, fields, inherits, generics);
            }

            return base.ParseTypeDef_Post(name, generics);
        }

        protected override IStatement ParseStatement()
        {
            if (Now([(null, VKeyword.FINAL.ToToken())], true))
            {
                Require(Keyword.VARIABLE.ToToken(), "'var'");

                var token = ParseVar();
                token.Meta["final"] = true;

                return token;
            }

            return base.ParseStatement();
        }
    }
}
