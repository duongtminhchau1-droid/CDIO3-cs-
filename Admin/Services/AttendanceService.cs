using Admin.Data;
using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Services
{
    public class AttendanceService
    {
        private readonly AppDbContext _db;

        private readonly TimeSpan WORK_START = new(8, 30, 0);
        private readonly TimeSpan WORK_END = new(17, 30, 0);

        public AttendanceService(AppDbContext db)
        {
            _db = db;
        }

        // ================= CHECK-IN =================
        public Attendance CheckIn(int empId)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            var record = _db.Attendances
                .FirstOrDefault(a => a.EmployeeId == empId && a.Date == today);

            if (record != null)
                return record;

            record = new Attendance
            {
                EmployeeId = empId,
                Date = today,
                CheckInTime = now,
                IsLate = now.TimeOfDay > WORK_START,
                Status = "Working"
            };

            _db.Attendances.Add(record);
            _db.SaveChanges();
            return record;
        }

        // ================= CHECK-OUT =================
        public Attendance? CheckOut(int empId)
        {
            var today = DateTime.Today;
            var now = DateTime.Now;

            var record = _db.Attendances.FirstOrDefault(a =>
                a.EmployeeId == empId &&
                a.Date == today &&
                a.Status == "Working"
            );

            if (record == null || record.CheckOutTime != null)
                return null;

            record.CheckOutTime = now;
            record.IsEarlyLeave = now.TimeOfDay < WORK_END;
            record.Status = "Completed";

            _db.SaveChanges();
            return record;
        }

        // ================= DAILY =================
        public List<object> GetDaily(DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;

            return _db.Attendances
                .AsNoTracking()
                .Where(a => a.Date == targetDate)
                .Select(a => new
                {
                    a.Id,
                    a.EmployeeId,
                    a.Date,
                    a.CheckInTime,
                    a.CheckOutTime,
                    a.IsLate,
                    a.IsEarlyLeave,
                    a.Status,
                    TotalMinutes =
                        a.CheckInTime != null && a.CheckOutTime != null
                            ? EF.Functions.DateDiffMinute(a.CheckInTime, a.CheckOutTime)
                            : 0
                })
                .OrderBy(x => x.CheckInTime)
                .Cast<object>()
                .ToList();
        }

        // ================= SUMMARY =================
        public object GetDailySummary(DateTime? date)
        {
            var targetDate = date ?? DateTime.Today;

            var data = _db.Attendances
                .Where(a => a.Date == targetDate)
                .ToList();

            return new
            {
                Total = data.Count,
                OnTime = data.Count(x => !x.IsLate),
                Late = data.Count(x => x.IsLate),
                Absent = data.Count(x => x.Status == "Absent"),
                Completed = data.Count(x => x.Status == "Completed")
            };
        }

        // ================= MONTHLY REPORT =================
        public List<object> GetMonthlyReport(int year, int month)
        {
            return _db.Attendances
                .Where(a => a.Date.Year == year && a.Date.Month == month)
                .GroupBy(a => a.EmployeeId)
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    WorkingDays = g.Count(x => x.Status == "Completed"),
                    LateDays = g.Count(x => x.IsLate),
                    EarlyLeaveDays = g.Count(x => x.IsEarlyLeave),

                    // ✅ FIX CHUẨN EF – KHÔNG Math.Round
                    TotalHours =
                        g.Sum(x =>
                            x.CheckInTime != null && x.CheckOutTime != null
                                ? EF.Functions.DateDiffMinute(x.CheckInTime, x.CheckOutTime)
                                : 0
                        ) / 60.0
                })
                .Cast<object>()
                .ToList();
        }
        public void MarkAbsentIfNeeded()
        {
            var today = DateTime.Today;

            var employees = _db.Employees.Select(e => e.Id).ToList();

            foreach (var empId in employees)
            {
                var hasAttendance = _db.Attendances
                    .Any(a => a.EmployeeId == empId && a.Date == today);

                var onLeave = _db.LeaveRequests.Any(l =>
                    l.EmployeeId == empId &&
                    l.Status == "Đã duyệt" &&
                    today >= l.StartDate.Date &&
                    today <= l.EndDate.Date
                );

                if (!hasAttendance && !onLeave)
                {
                    _db.Attendances.Add(new Attendance
                    {
                        EmployeeId = empId,
                        Date = today,
                        Status = "Absent"
                    });
                }
            }

            _db.SaveChanges();
        }

    }
}
