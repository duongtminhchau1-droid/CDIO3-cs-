using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        // DB: full_name
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        // DB: phone
        public string? Phone { get; set; }

        // DB KHÔNG CÓ
        [NotMapped]
        public string? Address { get; set; }

        // DB: status
        public bool Status { get; set; }

        // DB KHÔNG CÓ
        [NotMapped]
        public DateTime CreatedAt { get; set; }
    }
}
