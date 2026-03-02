using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Authorize]
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
            return Ok(_service.CheckIn(empId));
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

        // DAILY LIST
        [HttpGet]
        public IActionResult GetDaily([FromQuery] DateTime? date)
        {
            return Ok(_service.GetDaily(date));
        }

        // DASHBOARD SUMMARY
        [HttpGet("summary")]
        public IActionResult Summary([FromQuery] DateTime? date)
        {
            return Ok(_service.GetDailySummary(date));
        }

        // MONTHLY REPORT
        [HttpGet("report")]
        public IActionResult MonthlyReport(int year, int month)
        {
            return Ok(_service.GetMonthlyReport(year, month));
        }
    }
}
