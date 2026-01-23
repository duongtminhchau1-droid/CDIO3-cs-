using Admin.Data;
using Admin.Models;

namespace Admin.Services
{
    public class AttendanceService
    {
        private readonly AppDbContext _db;

        public AttendanceService(AppDbContext db)
        {
            _db = db;
        }

        public Attendance CheckIn(int empId)
        {
            var record = new Attendance
            {
                EmployeeId = empId,
                Date = DateTime.Today,
                Status = "Present"
            };

            _db.Attendances.Add(record);
            _db.SaveChanges();
            return record;
        }
    }

}
