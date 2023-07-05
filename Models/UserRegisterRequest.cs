using System.ComponentModel.DataAnnotations;

namespace simpleapi.Models
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 character.")]
    }
}
