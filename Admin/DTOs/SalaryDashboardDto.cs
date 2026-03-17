namespace Admin.DTOs
{
    public class SalaryDashboardDto
    {
        public decimal TotalNet { get; set; }
        public decimal AvgNet { get; set; }
        public int Pending { get; set; }   // bạn đang dùng trong FE
        public int Count { get; set; }
    }
}