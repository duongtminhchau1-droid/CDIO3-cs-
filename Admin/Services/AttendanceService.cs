using Admin.Data;
using Admin.DTOs;
using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Services
{
    public class AttendanceService
    {
        private readonly AppDbContext _db;

        public AttendanceService(AppDbContext db)
        {
            _db = db;
        }

        public object CheckIn(int empId)
        {
            var employeeExists = _db.Employees.Any(x => x.Id == empId);
            if (!employeeExists)
            {
                return new
                {
                    success = false,
                    message = "Không tìm thấy nhân viên"
                };
            }

            var today = DateTime.Today;
            var now = DateTime.Now;

            var attendance = _db.Attendances
                .FirstOrDefault(x => x.EmployeeId == empId && x.Date.Date == today);

            if (attendance != null && attendance.CheckInTime != null)
            {
                return new
                {
                    success = false,
                    message = "Nhân viên đã check-in hôm nay",
                    data = attendance
                };
            }

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    EmployeeId = empId,
                    Date = today,
                    CheckInTime = now,
                    CheckOutTime = null,
                    TotalHours = 0,
                    IsLate = now.TimeOfDay > new TimeSpan(8, 0, 0),
                    IsEarlyLeave = false,
                    Note = "-",
                    Status = "Working"
                };

                _db.Attendances.Add(attendance);
            }
            else
            {
                attendance.CheckInTime = now;
                attendance.IsLate = now.TimeOfDay > new TimeSpan(8, 0, 0);
                attendance.IsEarlyLeave = false;
                attendance.Note = string.IsNullOrWhiteSpace(attendance.Note) ? "-" : attendance.Note;
                attendance.Status = "Working";
            }

            _db.SaveChanges();

            return new
            {
                success = true,
                message = "Check-in thành công",
                data = attendance
            };
        }

        public object? CheckOut(int empId)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            var attendance = _db.Attendances
                .FirstOrDefault(x => x.EmployeeId == empId && x.Date.Date == today);

            if (attendance == null || attendance.CheckInTime == null || attendance.CheckOutTime != null)
                return null;

            attendance.CheckOutTime = now;
            attendance.IsEarlyLeave = now.TimeOfDay < new TimeSpan(17, 0, 0);
            attendance.Status = "Completed";

            var totalHours = Math.Round((now - attendance.CheckInTime.Value).TotalHours, 2);
            SetTotalHours(attendance, totalHours);

            _db.SaveChanges();

            return new
            {
                success = true,
                message = "Check-out thành công",
                data = attendance
            };
        }

        public List<AttendanceDto> GetByDate(DateTime date)
        {
            var query =
                from a in _db.Attendances.AsNoTracking()
                join e in _db.Employees.Include(x => x.Department).AsNoTracking()
                    on a.EmployeeId equals e.Id into empJoin
                from e in empJoin.DefaultIfEmpty()
                where a.Date.Date == date.Date
                select new AttendanceDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = e != null ? e.FullName : ("NV " + a.EmployeeId),
                    Department = e != null && e.Department != null ? e.Department.Name : "Unknown",
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    TotalHours = a.TotalHours,
                    IsLate = a.IsLate,
                    IsEarlyLeave = a.IsEarlyLeave,
                    Note = string.IsNullOrWhiteSpace(a.Note) ? "-" : a.Note,
                    Status = a.Status
                };

            return query.ToList();
        }

        public object GetSummary(DateTime date)
        {
            var data = _db.Attendances.Where(x => x.Date.Date == date.Date);

            return new
            {
                total = data.Count(),
                ontime = data.Count(x => x.Status == "Completed" && !x.IsLate && !x.IsEarlyLeave),
                late = data.Count(x => x.IsLate),
                missing = data.Count(x => x.Status == "Absent")
            };
        }

        public object GetMonthlyReport(int year, int month)
        {
            var rawData =
                (from a in _db.Attendances.AsNoTracking()
                 join e in _db.Employees.Include(x => x.Department).AsNoTracking()
                    on a.EmployeeId equals e.Id into empJoin
                 from e in empJoin.DefaultIfEmpty()
                 where a.Date.Year == year && a.Date.Month == month
                 select new
                 {
                     a.EmployeeId,
                     EmployeeName = e != null ? e.FullName : ("NV " + a.EmployeeId),
                     Department = e != null && e.Department != null ? e.Department.Name : "Unknown",
                     a.Status,
                     a.IsLate,
                     a.IsEarlyLeave,
                     a.TotalHours
                 }).ToList();

            var result = rawData
                .GroupBy(x => new { x.EmployeeId, x.EmployeeName, x.Department })
                .Select(g => new
                {
                    employeeId = g.Key.EmployeeId,
                    employeeName = g.Key.EmployeeName,
                    department = g.Key.Department,
                    totalDays = g.Count(),
                    completedDays = g.Count(x => x.Status == "Completed"),
                    lateDays = g.Count(x => x.IsLate),
                    earlyLeaveDays = g.Count(x => x.IsEarlyLeave),
                    totalHours = Math.Round(g.Sum(x => x.TotalHours), 2)
                })
                .OrderBy(x => x.employeeName)
                .ToList();

            return result;
        }

        private static void SetTotalHours(object attendance, double totalHours)
        {
            var prop = attendance.GetType().GetProperty("TotalHours");
            if (prop == null || !prop.CanWrite) return;

            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            var convertedValue = Convert.ChangeType(totalHours, targetType);
            prop.SetValue(attendance, convertedValue);
        }
    }
}