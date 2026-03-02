namespace Admin.DTOs
{
    public class SalaryDetailDto
    {
        public int PayrollId { get; set; }
        public string EmployeeName { get; set; } = "";
        public string EmployeeCode { get; set; } = "";
        public string Position { get; set; } = "";
        public string Department { get; set; } = "";

        public int WorkingDays { get; set; }
        public decimal OTHours { get; set; }
        public decimal OTRate { get; set; }

        public decimal SalaryBase { get; set; }
        public decimal Allowance { get; set; }
        public decimal OTAmount { get; set; }
        public decimal Bonus { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal BHXH { get; set; }
        public decimal BHYT { get; set; }
        public decimal BHTN { get; set; }
        public decimal PIT { get; set; }
        public decimal Advance { get; set; }
        public decimal Penalty { get; set; }

        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }

        public string Status { get; set; } = "";

        public DateTime CalculatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
