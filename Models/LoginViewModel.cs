using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username-ul este obligatoriu")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}