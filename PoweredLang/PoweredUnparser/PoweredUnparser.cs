using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Statement;
using GoPowered.Lang.Parser.Token.Statement.Implementation;
using GoPowered.Lang.Unparser;
using GoPowered.PoweredLang.PoweredParser.Token.Expr.Part;
using GoPowered.PoweredLang.PoweredParser.Token.Object;
using System.Runtime.CompilerServices;

namespace GoPowered.PoweredLang.PoweredUnparser
{
    public class PoweredUnparser(Parser input)
               : Unparser(input)
    {
        protected string endCode = "";

        public override string Unparse()
        {
            return base.Unparse() + (endCode == "" ? "" : $"\n\n// gopowered generated code //\n{endCode}");
        }

        protected override void Unparse1(IParserToken tok)
        {
            if (tok is PTTypeObject obj)
            {
                HandleTypeObject(obj);
            }
            else
            {
                base.Unparse1(tok);
            }
        }

        protected void HandleTypeObject(PTTypeObject typeObject)
        {
            output += "\n";
            output += "type ";
            output += typeObject.Name;

            if (typeObject.Generics != null)
                output += HandleFuncGenerics(typeObject.Generics);

            output += " ";
            output += "struct {";
            output += HandleInherits(typeObject.Inherits);
            output += HandleFields(typeObject.Fields);
            output += "\n}";
        }

        protected override string HandleAssign(StmtAssign stmt)
        {
            if (stmt.Value != null && stmt.Value is Expression expr && !expr.Singular && expr.Parts!.Count >= 1 && expr.Parts![^1] is EPNoError noerr)
            {
                expr.Parts.RemoveAt(expr.Parts.Count - 1);

                var errors = new List<string>();

                for (int i = 0; i < noerr.Errors; i++)
                    errors.Add("err" + i);

                var names = new List<string?>(errors);
                names.Insert(0, stmt.Name);

                var value = base.HandleExtractAssign(new StmtExtractAssign(names, stmt.Value, null));

                foreach (var name in errors)
                {
                    value += $"\n\tif ({name} != nil) {{" +
                             $"\n\t\tpanic(\"Expected a non-error value; got error instead: \" + {name})" +
                              "\n\t}";
                }

                return value;
            }

            return base.HandleAssign(stmt);
        }

        protected override string HandleExtractAssign(StmtExtractAssign stmt)
        {
            if (stmt.Value != null && stmt.Value is Expression expr && !expr.Singular && expr.Parts![^1] is EPNoError noerr)
            {
                expr.Parts.RemoveAt(expr.Parts.Count - 1);

                var errors = new List<string>();

                for (int i = 0; i < noerr.Errors; i++)
                    errors.Add("err" + i);

                var names = new List<string?>(errors);
                names.InsertRange(0, stmt.Names);

                var value = base.HandleExtractAssign(new StmtExtractAssign(names, stmt.Value, null));

                foreach (var name in errors)
                {
                    value += $"\n\tif ({name} != nil) {{" +
                             $"\n\t\tpanic(\"Expected a non-error value; got error instead: \" + {name})" +
                              "\n\t}";
                }

                return value;
            }

            return base.HandleExtractAssign(stmt);
        }

        protected override void HandleExpressionPart(IExpressionPart part, ref string output)
        {
            if (part is EPNoError noerr)
            {
                throw new UnparserError("The no-error mark is only supported within assignments in a multi-statement scope");
            }
            else
            {
                base.HandleExpressionPart(part, ref output);
            }
        }
    }
}
