using Domain.Enums;

namespace Domain.Models.FAQChat;

public class MessageModel
{
    public SenderType Sender { get; set; }

    public string Message { get; set; }
}