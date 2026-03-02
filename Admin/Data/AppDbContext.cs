using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }


        // ✅ THÊM DÒNG NÀY
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Map đúng tên cột MySQL (snake_case)
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("attendances");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.EmployeeId).HasColumnName("employee_id");
                entity.Property(x => x.Date).HasColumnName("date");

                entity.Property(x => x.CheckInTime).HasColumnName("check_in_time");
                entity.Property(x => x.CheckOutTime).HasColumnName("check_out_time");

                entity.Property(x => x.IsLate).HasColumnName("is_late");
                entity.Property(x => x.IsEarlyLeave).HasColumnName("is_early_leave");
                entity.Property(x => x.Status).HasColumnName("status");
            });
        }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

    }
}
