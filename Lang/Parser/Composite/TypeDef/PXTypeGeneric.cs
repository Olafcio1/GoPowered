using GoPowered.Lang.Lexer.Token;
using GoPowered.Lang.Parser.Token.Object.Generic;

namespace GoPowered.Lang.Parser
{
    public partial class Parser
    {
        protected partial void ParseTypeGeneric(out List<GenericPossibility> Types)
        {
            Require(Operator.LCurly.ToToken(), "'{'");

            Types = [];

            var first = true;

            while (true)
            {
                ConsumeNewlines();

                if (Now([(null, Operator.RCurly.ToToken())], true))
                    break;
                else if (first)
                    first = false;
                else
                {
                    Require(Operator.VLine.ToToken(), "'|'");
                    ConsumeNewlines();
                }

                var inheritorsAllowed = Now([(null, Operator.Tilde.ToToken())], true);
                var type = ParseType()!;

                Types.Add(new GenericPossibility(type, inheritorsAllowed));
            }
        }
    }
}
