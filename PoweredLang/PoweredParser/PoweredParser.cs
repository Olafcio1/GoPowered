using GoPowered.Lang.Lexer;
using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Parser.Token.Statement.Implementation.Assign;
using GoPowered.Lang.Parser.Type;
using GoPowered.PoweredLang.PoweredLexer.Token;
using GoPowered.PoweredLang.PoweredParser.Token.Expr.Part;
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

        protected override IAnyExpression ParseExpression(bool allowMath = true, bool allowLogic = true, bool allowInit = true, bool constant = false, bool allowTypeCast = false)
        {
            var anyExpr = base.ParseExpression(allowMath, allowLogic, allowInit, constant, allowTypeCast);
            if (anyExpr is Expression expr && !expr.Singular && expr.Parts!.Count >= 1 && expr.Parts[^1] is EPCall)
            {
                if (Now([(null, Operator.LNot.ToToken())], consume: true))
                {
                    int times = 1;

                    while (Now([(null, Operator.LNot.ToToken())], consume: true))
                        times++;

                    expr.Parts.Add(new EPNoError(times));
                }
            }

            return anyExpr;
        }
    }
}
