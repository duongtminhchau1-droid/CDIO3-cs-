using Admin.Data;
using Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Services
{
    public class SalaryService
    {
        private readonly AppDbContext _db;
        private int created;

        public SalaryService(AppDbContext db)
        {
            _db = db;
        }

        // =========================
        // DASHBOARD (CHO FE)
        // =========================
        public object Dashboard(int month, int year)
        {
            var q = _db.Payrolls
                .Where(p => p.Month == month && p.Year == year);

            return new
            {
                totalNet = q.Sum(x => x.TotalSalary),
                avgNet = q.Any() ? q.Average(x => x.TotalSalary) : 0,
                pending = q.Count(x => x.Status != "Đã thanh toán"),
                count = q.Count()
            };
        }

        // =========================
        // GET LIST + FILTER
        // =========================
        public List<Payroll> GetAll(int month, int year, string? status)
        {
            var q = _db.Payrolls
                .AsNoTracking()
                .Where(p => p.Month == month && p.Year == year);

            if (!string.IsNullOrEmpty(status) && status != "Tất cả")
                q = q.Where(p => p.Status == status);

            return q.ToList();
        }

        // =========================
        // TÍNH LƯƠNG THÁNG
        // =========================
        public int CalculateMonthly(int month, int year)
        {
            var employees = _db.Employees
                .AsNoTracking()
                .Select(e => new { e.Id }) // chỉ lấy Id
                .ToList();


            foreach (var e in employees)
            {
                bool exists = _db.Payrolls.Any(p =>
                    p.EmployeeId == e.Id &&
                    p.Month == month &&
                    p.Year == year);

                if (exists) continue;

                // 🔥 CHUẨN HRM:
                // Employee KHÔNG lưu lương
                // Payroll lưu snapshot lương theo tháng
                decimal salaryBase = 0;
                decimal bonus = 0;
                decimal deductions = 0;

                var payroll = new Payroll
                {
                    EmployeeId = e.Id,
                    Month = month,
                    Year = year,
                    SalaryBase = salaryBase,
                    Bonus = bonus,
                    Deductions = deductions,
                    TotalSalary = salaryBase + bonus - deductions,
                    Status = "Chờ duyệt"
                };

                _db.Payrolls.Add(payroll);
                created++;
            }

            _db.SaveChanges();
            return created;
        }

        // =========================
        // DUYỆT
        // =========================
        public bool Approve(int id)
        {
            var p = _db.Payrolls.Find(id);
            if (p == null || p.Status != "Chờ duyệt") return false;

            p.Status = "Chờ thanh toán";
            _db.SaveChanges();
            return true;
        }

        // =========================
        // THANH TOÁN
        // =========================
        public bool Pay(int id)
        {
            var p = _db.Payrolls.Find(id);
            if (p == null || p.Status != "Chờ thanh toán") return false;

            p.Status = "Đã thanh toán";
            _db.SaveChanges();
            return true;
        }
    }

}
