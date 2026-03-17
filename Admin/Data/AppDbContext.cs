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

        // ✅ THÊM 2 BẢNG DANH MỤC
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // EMPLOYEE
            // =========================
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");   // 👈 QUAN TRỌNG
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Role).HasColumnName("role");
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Status).HasColumnName("status");

                // ✅ BỔ SUNG ĐẦY ĐỦ THEO MYSQL
                entity.Property(e => e.DepartmentId).HasColumnName("department_id");
                entity.Property(e => e.PositionId).HasColumnName("position_id");
                entity.Property(e => e.SalaryBase).HasColumnName("salary_base");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // =========================
            // DEPARTMENT
            // =========================
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments");
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Id).HasColumnName("id");
                entity.Property(d => d.Name).HasColumnName("name");
                entity.Property(d => d.Description).HasColumnName("description");
            });

            // =========================
            // POSITION
            // =========================
            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("positions");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Title).HasColumnName("title");
                entity.Property(p => p.BaseSalaryRange).HasColumnName("base_salary_range");
            });

            // =========================
            // FK: Employee -> Department, Position
            // =========================
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId);

            // =========================
            // ATTENDANCE (giữ nguyên)
            // =========================
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
    }
}