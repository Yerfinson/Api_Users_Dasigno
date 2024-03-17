using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace WebApiDasigno_Users.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_User { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The First Name field cannot exceed 50 characters.")]
        public required string First_Name { get; set; }

        [StringLength(50, ErrorMessage = "The Middle Name field cannot exceed 50 characters.")]
        public string? Middle_Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The Last Name field cannot exceed 50 characters.")]
        public required string Last_Name { get; set; }

        [StringLength(50, ErrorMessage = "The Second Last Name field cannot exceed 50 characters.")]
        public string? Second_Last_Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date_of_birth { get; set; }

        [Required]
        public int? Salary {  get; set; }

        public DateTime Created_date { get; set; }
        public DateTime Update_date { get; set; }
    }
}
