using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class LeaveRequestCreateDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required]
        public string StartDate { get; set; } = null!; // dd/MM/yyyy

        [Required]
        public string EndDate { get; set; } = null!;

        public string Reason { get; set; } = "";
    }
}
