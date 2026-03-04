namespace Admin.Models
{
    public class EmployeeUpdateDto
    {
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string Status { get; set; } = "Đang làm việc";

        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public decimal? SalaryBase { get; set; }
    }
}