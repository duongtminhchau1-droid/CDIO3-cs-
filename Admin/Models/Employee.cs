using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("employees")]
    public class Employee
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("password")]
        public string Password { get; set; } = null!;

        [Column("role")]
        public string Role { get; set; } = null!;

        // DB: full_name
        [Column("full_name")]
        public string FullName { get; set; } = null!;

        // DB: phone
        [Column("phone")]
        public string? Phone { get; set; }

        // DB: department_id
        [Column("department_id")]
        public int? DepartmentId { get; set; }

        // DB: position_id
        [Column("position_id")]
        public int? PositionId { get; set; }

        // DB: salary_base
        [Column("salary_base", TypeName = "decimal(15,2)")]
        public decimal? SalaryBase { get; set; }

        // DB: status (enum 'Đang làm việc','Đã nghỉ việc')
        [Column("status")]
        public string Status { get; set; } = "Đang làm việc";

        // DB: created_at
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // DB KHÔNG CÓ
        [NotMapped]
        public string? Address { get; set; }

        // Navigation (để join lấy tên phòng ban/chức vụ)
        public Department? Department { get; set; }
        public Position? Position { get; set; }

    }
}