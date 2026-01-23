namespace Admin.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        // ADMIN / EMPLOYEE
        public string Role { get; set; } = "EMPLOYEE";

        // các field khác bạn có thì giữ nguyên
    }
}
