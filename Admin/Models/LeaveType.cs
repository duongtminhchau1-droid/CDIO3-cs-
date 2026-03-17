using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    [Table("leave_types")]
    public class LeaveType
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";
    }
}