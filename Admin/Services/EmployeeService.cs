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
        // GET ALL
        // =========================
        public List<Employee> GetAll()
        {
            return _db.Employees
                .AsNoTracking()
                .Select(e => new Employee
                {
                    Id = e.Id,
                    Email = e.Email,
                    Role = e.Role
                    // 
                })
                .ToList();
        }

        // =========================
        // GET BY ID
        // =========================
        public Employee? GetById(int id)
        {
            return _db.Employees
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new Employee
                {
                    Id = e.Id,
                    Email = e.Email,
                    Role = e.Role
                    // ❌ BỎ CreatedAt
                })
                .FirstOrDefault();
        }
        public Employee Create(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                employee.FullName = "EMPLOYEE";
            }

            _db.Employees.Add(employee);
            _db.SaveChanges();

            return employee;
        }




        // =========================
        // UPDATE
        // =========================
        public bool Update(int id, Employee employee)
        {
            var existing = _db.Employees.Find(id);
            if (existing == null)
                return false;

            existing.Email = employee.Email;
            existing.Role = employee.Role;

            _db.SaveChanges();
            return true;
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================
        public bool Delete(int id)
        {
            var emp = _db.Employees.Find(id);
            if (emp == null)
                return false;

            //  KHÔNG xóa cứng
            // emp.Status: true = active, false = deleted
            emp.Status = false;

            _db.SaveChanges();
            return true;
        }
    }
}
