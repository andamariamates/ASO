using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username-ul este obligatoriu")]
        [MinLength(3, ErrorMessage = "Minim 3 caractere")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Minim 3 caractere")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirmarea parolei este obligatorie")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolele nu coincid")]
        public required string ConfirmPassword { get; set; }
    }
}