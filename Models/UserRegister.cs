using System.ComponentModel.DataAnnotations; // This is for validations

namespace test.Models
{
    public class UserRegister: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="Only letters for name")]
        public string firstname {get; set;}

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage="Only letters for name")]
        public string lastname {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string password {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password's do not match")]
        public string confirm {get; set;}

        [Required]
        [EmailAddress]
        public string email {get; set;}

    }
}