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

        [HttpPost("checkin/{empId}")]
        public IActionResult CheckIn(int empId)
        {
            return Ok(_service.CheckIn(empId));
        }
    }

}
