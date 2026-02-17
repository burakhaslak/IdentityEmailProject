namespace ProjectEmailWithIdentity.Models
{
    public class TopBarMessageViewModel
    {
        public int UnreadCount { get; set; }
        public List<ProjectEmailWithIdentity.Entities.Message> UnreadMessages { get; set; }
    }
}
