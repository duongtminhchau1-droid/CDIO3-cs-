using Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [ApiController]
    [Route("api/admin/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _service;

        public AttendanceController(AttendanceService service)
        {
            _service = service;
        }

        // CHECK-IN
        [HttpPost("checkin/{empId}")]
        public IActionResult CheckIn(int empId)
        {
            var result = _service.CheckIn(empId);
            return Ok(result);
        }

        // CHECK-OUT
        [HttpPost("checkout/{empId}")]
        public IActionResult CheckOut(int empId)
        {
            var result = _service.CheckOut(empId);

            if (result == null)
                return BadRequest("Chưa check-in hoặc đã check-out");

            return Ok(result);
        }

        // DAILY LIST - GIỮ PHẦN MỚI
        [HttpGet]
        public IActionResult GetByDate([FromQuery] string? date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return BadRequest("Thiếu date");

            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Date không hợp lệ");

            var result = _service.GetByDate(parsedDate);
            return Ok(result);
        }

        // SUMMARY - GIỮ PHẦN MỚI
        [HttpGet("summary")]
        public IActionResult GetSummary([FromQuery] string? date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return BadRequest("Thiếu date");

            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Date không hợp lệ");

            var result = _service.GetSummary(parsedDate);
            return Ok(result);
        }

        // MONTHLY REPORT - GIỮ API CŨ
        [HttpGet("report")]
        public IActionResult MonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            var result = _service.GetMonthlyReport(year, month);
            return Ok(result);
        }
    }
}