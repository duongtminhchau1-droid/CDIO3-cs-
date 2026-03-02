using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("payroll")]
    public class Payroll
    {
        [Key]
        public int Id { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        [Column("salary_base")]
        public decimal SalaryBase { get; set; }

        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; }

        [Column("total_salary")]
        public decimal TotalSalary { get; set; }

        public string Status { get; set; } = "Chưa tính";
    }
}
