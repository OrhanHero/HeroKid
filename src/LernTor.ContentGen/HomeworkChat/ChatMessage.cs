namespace LernTor.ContentGen.HomeworkChat;

/// <summary>Eine einzelne Nachricht im KI-Lernchat zu einer Aufgabe (siehe <see cref="IHomeworkHelpChatService"/>).</summary>
public sealed class ChatMessage
{
    public required ChatRole Role { get; init; }
    public required string Text { get; init; }
}
