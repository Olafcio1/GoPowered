using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Expr;
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
            return base.Unparse() + (endCode == "" ? "" : $"\n\n# gopowered generated code #\n{endCode}");
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

        private int requireNotNull_counter = 0;
        private int requireNotNull_variables = 1;

        protected override void HandleExpressionPart(IExpressionPart part, ref string output)
        {
            if (part is EPNoError noerr)
            {
                var index = requireNotNull_counter++;
                var prevoutput = output;

                output = $"__callback_{index}()";

                var outputs = "output";
                var generics = "T";

                if (requireNotNull_variables > 1)
                {
                    for (int i = 0; i < requireNotNull_variables - 1; i++)
                    {
                        outputs += ", ";
                        outputs += "output";
                        outputs += i;

                        generics += ", ";
                        generics += "T";
                        generics += i;
                    }

                    requireNotNull_variables = 1;
                }

                endCode += $"func __callback_{index}[{generics}]() {(generics.Equals("T") ? generics : $"({ generics })")} {{" +
                           $"\n\t";

                endCode += outputs;

                for (int i = 0; i < noerr.Errors; i++)
                {
                    endCode += ", ";
                    endCode += "err";
                    endCode += i;
                }

                endCode += " := ";
                endCode += prevoutput;

                for (int i = 0; i < noerr.Errors; i++)
                {
                    endCode += $"\n\tif (err{i} != nil) {{" +
                               $"\n\t\tpanic(\"Expected a non-error value; got error instead: \" + err{i})" +
                                "\n\t}";
                }

                endCode += "\n\treturn " + outputs;
                endCode += "\n}\n";
            }
            else
            {
                base.HandleExpressionPart(part, ref output);
            }
        }

        protected override string HandleExtractAssign(StmtExtractAssign stmt)
        {
            requireNotNull_variables = stmt.Names.Count;
            return base.HandleExtractAssign(stmt);
        }

        protected override string HandleSetExtract(StmtExtractSet stmt)
        {
            requireNotNull_variables = stmt.ExtractTo.Count;
            return base.HandleSetExtract(stmt);
        }
    }
}
