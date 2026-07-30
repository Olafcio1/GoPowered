using GoPowered.Lang.Parser;
using GoPowered.Lang.Parser.Token.Expr;
using GoPowered.Lang.Parser.Token.Expr.Part;
using GoPowered.Lang.Parser.Token.Expr.Target.Single;

namespace GoPowered.Lang.Unparser
{
    public partial class Unparser
    {
        protected string HandleExpression(Expression expr)
        {
            var output = "";

            if (expr.Pointers > 0)
            {
                for (int i = 0; i < expr.Pointers; i++)
                    output += "*";
            }
            else
            {
                for (int i = 0; i < expr.Pointers; i++)
                    output += "&";
            }

            HandleExpressionTarget(expr.Target, ref output);

            if (!expr.Singular)
            {
                foreach (var part in expr.Parts!)
                {
                    HandleExpressionPart(part, ref output);
                }
            }

            return output;
        }

        protected virtual void HandleExpressionTarget(IExpressionTarget target, ref string output)
        {
                 if (HandleSingular(target, ref output));
            else if (HandleClosure(target, ref output));
            else if (HandleMake(target, ref output));
            else if (HandleReference(target, ref output));
            else if (HandleNest(target, ref output));
            else if (HandleReceive(target, ref output));
            else if (HandleConvert(target, ref output));
            else if (HandleMap(target, ref output));
            else if (HandleSlice(target, ref output));
            else if (HandleImplicitStruct(target, ref output));
            else
                throw new UnparserError("Unexpected expression target '" + TypeOf(target).Substring(3) + "'");
        }

        protected virtual void HandleExpressionPart(IExpressionPart part, ref string output)
        {
                 if (HandleSquare(part, ref output));
            else if (HandleCast(part, ref output));
            else if (HandleMember(part, ref output));
            else if (HandleCall(part, ref output));
            else if (HandleNew(part, ref output));
            else if (HandleSlice(part, ref output));
            else
                throw new UnparserError("Unexpected expression part '" + TypeOf(part).Substring(3) + "'");
        }

        protected partial bool HandleClosure(IExpressionTarget target, ref string output);
        protected partial bool HandleMake(IExpressionTarget target, ref string output);
        protected partial bool HandleReference(IExpressionTarget target, ref string output);
        protected partial bool HandleNest(IExpressionTarget target, ref string output);
        protected partial bool HandleReceive(IExpressionTarget target, ref string output);
        protected partial bool HandleConvert(IExpressionTarget target, ref string output);
        protected partial bool HandleMap(IExpressionTarget target, ref string output);
        protected partial bool HandleSlice(IExpressionTarget target, ref string output);
        protected partial bool HandleImplicitStruct(IExpressionTarget target, ref string output);

        protected partial bool HandleSquare(IExpressionPart part, ref string output);
        protected partial bool HandleCast(IExpressionPart part, ref string output);
        protected partial bool HandleMember(IExpressionPart part, ref string output);
        protected partial bool HandleCall(IExpressionPart part, ref string output);
        protected partial bool HandleNew(IExpressionPart part, ref string output);
        protected partial bool HandleSlice(IExpressionPart part, ref string output);

        protected bool HandleSingular(IExpressionTarget target, ref string output)
        {
            if (target is ESTBoolean boolean)
                output += HandleESTBoolean(boolean);
            else if (target is ESTInteger integer)
                output += HandleESTInteger(integer);
            else if (target is ESTFloat @float)
                output += HandleESTFloat(@float);
            else if (target is ESTChar @char)
                output += HandleESTChar(@char);
            else if (target is ESTString @string)
                output += HandleESTString(@string);
            else if (target == ESTNil.INSTANCE)
                output += HandleESTNil();
            else return false;

            return true;
        }

        protected partial string HandleESTBoolean(ESTBoolean boolean);
        protected partial string HandleESTInteger(ESTInteger integer);
        protected partial string HandleESTFloat(ESTFloat @float);
        protected partial string HandleESTChar(ESTChar @char);
        protected partial string HandleESTString(ESTString @string);
        protected partial string HandleESTNil();
    }
}
