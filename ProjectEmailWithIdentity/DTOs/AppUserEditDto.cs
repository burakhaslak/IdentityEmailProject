namespace ProjectEmailWithIdentity.DTOs
{
    public class AppUserEditDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? ImageUrl { get; set; }
        public string? About { get; set; }
        public string? CurrentPassword { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Mail { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LocationUrl { get; set; }
        public IFormFile ImagePath {  get; set; }
    }
}
