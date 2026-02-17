using System.ComponentModel.DataAnnotations;

namespace ProjectEmailWithIdentity.DTOs
{
    public class CreateUserRegisterDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? UserName { get; set; }
        public string? EMail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? ConfirmCode { get; set; }

    }
}
