using Admin.Models;
using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    [Route("api/admin/salaries")]
    public class SalaryController : ControllerBase
    {
        private readonly SalaryService _service;

        public SalaryController(SalaryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_service.GetAll());

        [HttpPost]
        public IActionResult Create(Salary s) => Ok(_service.Create(s));
    }

}
