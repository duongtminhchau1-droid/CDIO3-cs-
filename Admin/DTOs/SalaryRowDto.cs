namespace Admin.DTOs
{
    public class SalaryRowDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }

        public decimal SalaryBase { get; set; }
        public decimal Bonus { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal NetPay { get; set; }

        public string Status { get; set; }
    }
}