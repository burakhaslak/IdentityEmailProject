using Microsoft.AspNetCore.Identity;

namespace ProjectEmailWithIdentity.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? ImageUrl { get; set; }
        public string? About { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? LocationUrl { get; set; }
        public int ConfirmCode { get; set; }
    }
}
