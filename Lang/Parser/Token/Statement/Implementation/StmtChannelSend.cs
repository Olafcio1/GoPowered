namespace GoPowered.Lang.Parser.Token.Statement.Implementation
{
    /**
     * Used to send a message to a channel.
     */
    public record StmtChannelSend(IAnyExpression Channel, IAnyExpression Message) : IStatement;
}
