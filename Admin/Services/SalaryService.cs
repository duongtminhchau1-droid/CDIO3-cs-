using Admin.Data;
using Admin.Models;

namespace Admin.Services
{
    public class SalaryService
    {
        private readonly AppDbContext _db;

        public SalaryService(AppDbContext db)
        {
            _db = db;
        }

        public List<Salary> GetAll() => _db.Salaries.ToList();

        public Salary Create(Salary s)
        {
            _db.Salaries.Add(s);
            _db.SaveChanges();
            return s;
        }
    }

}
