namespace Admin.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string FullName { get; set; } = "";
        public string? Phone { get; set; }
        public string Status { get; set; } = "";

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public int? PositionId { get; set; }
        public string? PositionTitle { get; set; }

        public decimal? SalaryBase { get; set; }
    }
}