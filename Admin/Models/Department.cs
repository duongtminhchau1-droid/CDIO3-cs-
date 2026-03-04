using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("departments")]
    public class Department
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}