using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("positions")]
    public class Position
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("base_salary_range")]
        public string? BaseSalaryRange { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}