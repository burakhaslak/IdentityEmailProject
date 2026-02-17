namespace ProjectEmailWithIdentity.DTOs
{
    public class ComposeMessageDto
    {
        public int MessageId { get; set; }
        public string ReceiverMail { get; set; }
        public string SenderMail { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsStatus { get; set; }
        public bool IsStarred { get; set; }
        public bool IsTrash { get; set; }
        public bool IsDrafted { get; set; }

    }
}
