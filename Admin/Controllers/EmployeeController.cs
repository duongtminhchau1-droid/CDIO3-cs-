using Admin.Models;
using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Authorize(Roles = "ADMIN,HR")]
    [ApiController]
    [Route("api/admin/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        // =========================
        // GET ALL
        // GET: api/admin/employees
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _service.GetAll();
            return Ok(employees);
        }

        // =========================
        // GET BY ID
        // GET: api/admin/employees/{id}
        // =========================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _service.GetById(id);

            if (employee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(employee);
        }

        // =========================
        // CREATE
        // POST: api/admin/employees
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.Create(employee);
            return Ok(created);
        }

        // =========================
        // UPDATE ✅ SỬA: dùng EmployeeUpdateDto để không bắt Password
        // PUT: api/admin/employees/{id}
        // =========================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _service.Update(id, dto);

            if (!ok)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Updated successfully" });
        }

        // =========================
        // DELETE
        // DELETE: api/admin/employees/{id}
        // =========================
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")] // chỉ ADMIN được xóa
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);

            if (!success)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}