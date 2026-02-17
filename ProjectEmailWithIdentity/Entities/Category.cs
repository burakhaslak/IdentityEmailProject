namespace ProjectEmailWithIdentity.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryColor { get; set; }

        public List<Message> Messages { get; set; }
    }
}
