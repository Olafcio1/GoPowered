namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    public record StmtExpression(Expr.Expression Expr) : IStatement;
}
