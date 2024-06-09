using System.ComponentModel.DataAnnotations;

namespace DeskJockey.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        public string Email { get; set; }
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
        public string Password { get; set; }

    }
}