using System.ComponentModel.DataAnnotations; // This is for validations

namespace test.Models
{
    public class UserLogin: BaseEntity
    {
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string LoginPassword {get; set;}

        [Required]
        [EmailAddress]
        public string LoginEmail {get; set;}

    }
}