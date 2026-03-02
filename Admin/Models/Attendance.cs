using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("attendances")]
    public class Attendance
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("check_in_time")]
        public DateTime? CheckInTime { get; set; }

        [Column("check_out_time")]
        public DateTime? CheckOutTime { get; set; }

        [Column("is_late")]
        public bool IsLate { get; set; }

        [Column("is_early_leave")]
        public bool IsEarlyLeave { get; set; }

        // Working | Completed | Absent
        [Column("status")]
        public string Status { get; set; } = "Working";
    }
}
