using Admin.Data;
using Admin.Models;

namespace Admin.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _db;

        public EmployeeService(AppDbContext db)
        {
            _db = db;
        }

        public List<Employee> GetAll() => _db.Employees.ToList();

        public Employee Create(Employee e)
        {
            _db.Employees.Add(e);
            _db.SaveChanges();
            return e;
        }

        internal bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        internal object Update(int id, Employee employee)
        {
            throw new NotImplementedException();
        }

        internal object GetById(int id)
        {
            throw new NotImplementedException();
        }
    }

}
