using Admin.Data;
using Admin.Models;
using Admin.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Admin.Services
{
    public class LeaveRequestService
    {
        private readonly AppDbContext _db;

        public LeaveRequestService(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // GET ALL
        // =========================
        public List<LeaveRequestDto> GetAll(string? status)
        {
            var query =
                from l in _db.LeaveRequests
                join e in _db.Employees on l.EmployeeId equals e.Id
                join t in _db.LeaveTypes on l.LeaveTypeId equals t.Id
                select new LeaveRequestDto
                {
                    Id = l.Id,
                    EmployeeId = e.Id,
                    EmployeeName = e.FullName,
                    LeaveType = t.Name,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    TotalDays = l.TotalDays,
                    Reason = l.Reason,
                    Status = l.Status
                };

            return query.ToList();
        }

        // =========================
        // CREATE
        // =========================
        public LeaveRequest Create(LeaveRequestCreateDto dto)
        {
            var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy" };

            if (!DateTime.TryParseExact(
                dto.StartDate,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var start))
            {
                throw new Exception("StartDate không đúng định dạng");
            }

            if (!DateTime.TryParseExact(
                dto.EndDate,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var end))
            {
                throw new Exception("EndDate không đúng định dạng");
            }

            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = start,
                EndDate = end,
                TotalDays = (end.Date - start.Date).Days + 1,
                Reason = dto.Reason,
                Status = "Chờ duyệt",
                CreatedAt = DateTime.Now
            };

            _db.Add(leave);
            _db.SaveChanges();

            return leave;
        }

        // =========================
        // APPROVE
        // =========================
        public bool Approve(int id)
        {
            var leave = _db.Set<LeaveRequest>().Find(id);
            if (leave == null) return false;

            leave.Status = "Đã duyệt";

            _db.SaveChanges();

            return true;
        }

        // =========================
        // REJECT
        // =========================
        public bool Reject(int id)
        {
            var leave = _db.Set<LeaveRequest>().Find(id);
            if (leave == null) return false;

            leave.Status = "Từ chối";

            _db.SaveChanges();

            return true;
        }

        // =========================
        // DASHBOARD
        // =========================
        public object Dashboard()
        {
            var today = DateTime.Today;

            return new
            {
                pending = _db.Set<LeaveRequest>().Count(l => l.Status == "Chờ duyệt"),
                approved = _db.Set<LeaveRequest>().Count(l => l.Status == "Đã duyệt"),
                rejected = _db.Set<LeaveRequest>().Count(l => l.Status == "Từ chối"),
                onLeaveToday = _db.Set<LeaveRequest>()
                    .Count(l =>
                        l.Status == "Đã duyệt"
                        && l.StartDate.Date <= today
                        && l.EndDate.Date >= today
                    )
            };
        }
    }
}