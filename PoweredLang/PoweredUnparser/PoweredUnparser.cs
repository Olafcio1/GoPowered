using GoPowered.Lang.Parser;
using GoPowered.Lang.Unparser;
using GoPowered.PoweredLang.PoweredParser.Token.Object;

namespace GoPowered.PoweredLang.PoweredUnparser
{
    public class PoweredUnparser(Parser input)
               : Unparser(input)
    {
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
    }
}
