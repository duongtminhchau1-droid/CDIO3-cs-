using Admin.Data;
using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _db;

        public EmployeeService(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // GET ALL (DTO có DepartmentName/PositionTitle)
        // =========================
        public async Task<List<EmployeeDto>> GetAll()
        {
            return await _db.Employees
                .AsNoTracking()
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Email = e.Email,
                    Role = e.Role,
                    FullName = e.FullName,
                    Phone = e.Phone,
                    Status = e.Status,

                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null,

                    PositionId = e.PositionId,
                    PositionTitle = e.Position != null ? e.Position.Title : null,

                    SalaryBase = e.SalaryBase
                })
                .ToListAsync();
        }

        // =========================
        // GET BY ID (DTO)
        // =========================
        public async Task<EmployeeDto?> GetById(int id)
        {
            return await _db.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Email = e.Email,
                    Role = e.Role,
                    FullName = e.FullName,
                    Phone = e.Phone,
                    Status = e.Status,

                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null,

                    PositionId = e.PositionId,
                    PositionTitle = e.Position != null ? e.Position.Title : null,

                    SalaryBase = e.SalaryBase
                })
                .FirstOrDefaultAsync();
        }

        // =========================
        // CREATE
        // =========================
        public async Task<Employee> Create(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName))
                employee.FullName = "EMPLOYEE";

            if (string.IsNullOrWhiteSpace(employee.Status))
                employee.Status = "Đang làm việc";

            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            return employee;
        }

        // =========================
        // UPDATE ✅ SỬA: dùng EmployeeUpdateDto (không bắt Password)
        // =========================
        public async Task<bool> Update(int id, EmployeeUpdateDto dto)
        {
            var existing = await _db.Employees.FindAsync(id);
            if (existing == null)
                return false;

            existing.Email = dto.Email;
            existing.Role = dto.Role;

            existing.FullName = dto.FullName;
            existing.Phone = dto.Phone;

            existing.Status = string.IsNullOrWhiteSpace(dto.Status)
                ? existing.Status
                : dto.Status;

            existing.DepartmentId = dto.DepartmentId;
            existing.PositionId = dto.PositionId;
            existing.SalaryBase = dto.SalaryBase;

            await _db.SaveChangesAsync();
            return true;
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public async Task<bool> Delete(int id)
        {
            var emp = await _db.Employees.FindAsync(id);
            if (emp == null)
                return false;

            emp.Status = "Đã nghỉ việc";
            await _db.SaveChangesAsync();
            return true;
        }

        // =========================
        // LIST DEPARTMENTS (CHO DROPDOWN FE)
        // =========================
        public async Task<List<object>> GetDepartments()
        {
            return await _db.Departments
                .AsNoTracking()
                .Select(d => new { d.Id, d.Name })
                .Cast<object>()
                .ToListAsync();
        }

        // =========================
        // LIST POSITIONS (CHO DROPDOWN FE)
        // =========================
        public async Task<List<object>> GetPositions()
        {
            return await _db.Positions
                .AsNoTracking()
                .Select(p => new { p.Id, p.Title })
                .Cast<object>()
                .ToListAsync();
        }
    }
}